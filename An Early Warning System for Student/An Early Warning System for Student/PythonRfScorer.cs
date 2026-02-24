using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace An_Early_Warning_System_for_Student
{
    internal static class PythonRfScorer
    {
        internal sealed record ScoreResult(bool Ok, double Probability, int Prediction, string? Error);

        public static ScoreResult Score(
            double gwa,
            int lowSubjectCount,
            int borderlineSubjectCount,
            int lowAttendanceSubjectCount,
            double minAttendanceRate
        )
        {
            string baseDir = AppContext.BaseDirectory;
            string scriptPath = Path.Combine(baseDir, "ModelData", "score_rf.py");

            if (!File.Exists(scriptPath))
                return new ScoreResult(false, 0, 0, $"Python scoring script not found: {scriptPath}");

            var payload = new
            {
                gwa,
                low_subject_count = lowSubjectCount,
                borderline_subject_count = borderlineSubjectCount,
                low_attendance_subject_count = lowAttendanceSubjectCount,
                min_attendance_rate = minAttendanceRate,
            };

            string jsonIn = JsonSerializer.Serialize(payload);

            // Prefer Python Launcher (py) if available; fall back to python.
            var firstTry = TryRunPython("py", $"-3 -u \"{scriptPath}\"", jsonIn);
            if (firstTry.Ok || firstTry.Error is not string)
                return firstTry;

            return TryRunPython("python", $"-u \"{scriptPath}\"", jsonIn);
        }

        private static ScoreResult TryRunPython(string exe, string args, string stdin)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = exe,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                };

                using var proc = Process.Start(psi);
                if (proc == null)
                    return new ScoreResult(false, 0, 0, "Failed to start Python process");

                proc.StandardInput.Write(stdin);
                proc.StandardInput.Close();

                // Small timeout so the UI doesn't hang forever.
                if (!proc.WaitForExit(10_000))
                {
                    try { proc.Kill(true); } catch { }
                    return new ScoreResult(false, 0, 0, "Python scoring timed out");
                }

                string stdout = proc.StandardOutput.ReadToEnd();
                string stderr = proc.StandardError.ReadToEnd();

                if (string.IsNullOrWhiteSpace(stdout))
                    return new ScoreResult(false, 0, 0, string.IsNullOrWhiteSpace(stderr) ? "No output from Python" : stderr.Trim());

                using var doc = JsonDocument.Parse(stdout);
                var root = doc.RootElement;

                bool ok = root.TryGetProperty("ok", out var okProp) && okProp.ValueKind == JsonValueKind.True;
                if (!ok)
                {
                    string err = root.TryGetProperty("error", out var errProp) ? errProp.GetString() ?? "Unknown error" : "Unknown error";
                    if (!string.IsNullOrWhiteSpace(stderr))
                        err = err + "\n" + stderr.Trim();
                    return new ScoreResult(false, 0, 0, err);
                }

                double prob = root.TryGetProperty("prob_at_risk", out var pProp)
                    ? pProp.GetDouble()
                    : 0.0;

                int pred = root.TryGetProperty("pred_at_risk", out var predProp)
                    ? predProp.GetInt32()
                    : (prob >= 0.5 ? 1 : 0);

                return new ScoreResult(true, prob, pred, null);
            }
            catch (Win32Exception)
            {
                return new ScoreResult(false, 0, 0, $"Python executable not found: {exe}");
            }
            catch (Exception ex)
            {
                return new ScoreResult(false, 0, 0, ex.Message);
            }
        }
    }
}
