using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace An_Early_Warning_System_for_Student
{
    internal sealed class AprioriRiskModel
    {
        internal sealed record RiskRule(HashSet<string> Antecedents, double Confidence, double Lift);

        internal sealed record RiskEvaluation(
            double Probability,
            IReadOnlyCollection<string> Tags,
            IReadOnlyCollection<string> MatchedAntecedents
        );

        private readonly List<RiskRule> _rules;

        private AprioriRiskModel(List<RiskRule> rules)
        {
            _rules = rules;
        }

        private static AprioriRiskModel? _instance;

        public static AprioriRiskModel? TryLoadDefault()
        {
            if (_instance != null)
                return _instance;

            try
            {
                string baseDir = AppContext.BaseDirectory;
                string rulesPath = Path.Combine(baseDir, "ModelData", "apriori_rules.csv");

                if (!File.Exists(rulesPath))
                    return null;

                var rules = LoadRulesFromCsv(rulesPath);
                _instance = new AprioriRiskModel(rules);
                return _instance;
            }
            catch
            {
                return null;
            }
        }

        public RiskEvaluation Evaluate(
            double gwa,
            double overallAttendanceAvg,
            int lowSubjectCount,
            int borderlineSubjectCount
        )
        {
            var tags = BuildTags(gwa, overallAttendanceAvg, lowSubjectCount, borderlineSubjectCount);

            double bestProb = 0.0;
            List<string> matched = new List<string>();

            foreach (var rule in _rules)
            {
                if (rule.Antecedents.All(tags.Contains))
                {
                    if (rule.Confidence > bestProb)
                    {
                        bestProb = rule.Confidence;
                        matched.Clear();
                        matched.Add(string.Join(", ", rule.Antecedents.OrderBy(x => x)));
                    }
                    else if (Math.Abs(rule.Confidence - bestProb) < 1e-9)
                    {
                        matched.Add(string.Join(", ", rule.Antecedents.OrderBy(x => x)));
                    }
                }
            }

            return new RiskEvaluation(bestProb, tags.ToList(), matched);
        }

        private static HashSet<string> BuildTags(
            double gwa,
            double overallAttendanceAvg,
            int lowSubjectCount,
            int borderlineSubjectCount
        )
        {
            var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (gwa > 0)
            {
                if (gwa < 75)
                    tags.Add("GWA_LOW");
                else if (gwa < 85)
                    tags.Add("GWA_MED");
                else
                    tags.Add("GWA_HIGH");
            }

            if (overallAttendanceAvg > 0)
            {
                if (overallAttendanceAvg < 75)
                    tags.Add("ATT_LOW");
                else
                    tags.Add("ATT_GOOD");
            }

            if (lowSubjectCount >= 2)
                tags.Add("LOW_SUBJECTS_2PLUS");

            if (borderlineSubjectCount >= 2)
                tags.Add("BORDERLINE_2PLUS");

            return tags;
        }

        private static List<RiskRule> LoadRulesFromCsv(string path)
        {
            var rules = new List<RiskRule>();

            using var stream = File.OpenRead(path);
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

            string? headerLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(headerLine))
                return rules;

            var headers = SplitCsvLine(headerLine).Select(h => h.Trim()).ToList();

            int idxAntecedents = headers.FindIndex(h => h.Equals("antecedents", StringComparison.OrdinalIgnoreCase));
            int idxConfidence = headers.FindIndex(h => h.Equals("confidence", StringComparison.OrdinalIgnoreCase));
            int idxLift = headers.FindIndex(h => h.Equals("lift", StringComparison.OrdinalIgnoreCase));

            if (idxAntecedents < 0 || idxConfidence < 0)
                return rules;

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var fields = SplitCsvLine(line);
                if (fields.Count <= Math.Max(idxAntecedents, idxConfidence))
                    continue;

                string antecedentsRaw = fields[idxAntecedents];
                string confidenceRaw = fields[idxConfidence];
                string liftRaw = (idxLift >= 0 && idxLift < fields.Count) ? fields[idxLift] : "0";

                if (!double.TryParse(confidenceRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out double confidence))
                    continue;

                double.TryParse(liftRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out double lift);

                var antecedents = ParseAntecedents(antecedentsRaw);
                if (antecedents.Count == 0)
                    continue;

                rules.Add(new RiskRule(antecedents, confidence, lift));
            }

            return rules;
        }

        private static HashSet<string> ParseAntecedents(string antecedentsRaw)
        {
            var tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Match match in Regex.Matches(antecedentsRaw, "'([^']+)'"))
            {
                var value = match.Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(value))
                    tags.Add(value);
            }

            return tags;
        }

        private static List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }

                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                    continue;
                }

                sb.Append(c);
            }

            result.Add(sb.ToString());
            return result;
        }
    }
}
