using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace An_Early_Warning_System_for_Student
{
    internal static class OtpEmailService
    {
        private sealed class SmtpConfig
        {
            public string Host { get; set; } = "smtp.gmail.com";
            public int Port { get; set; } = 587;
            public bool EnableSsl { get; set; } = true;
            public string User { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public static void SendOtp(string toEmail, string otp)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("Recipient email is required.", nameof(toEmail));

            var config = LoadConfig();

            var mail = new MailMessage
            {
                From = new MailAddress(config.User),
                Subject = "Your OTP Code - Early Warning System",
                Body = "Your OTP code is: " + otp + "\n\nThis code will expire in 5 minutes."
            };
            mail.To.Add(toEmail);

            using var smtp = new SmtpClient(config.Host)
            {
                Port = config.Port,
                EnableSsl = config.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(config.User, config.Password)
            };

            smtp.Send(mail);
        }

        private static SmtpConfig LoadConfig()
        {
            // 1) Environment variables (best for CI / no files)
            var envUser = Environment.GetEnvironmentVariable("SEEPATH_SMTP_USER");
            var envPass = Environment.GetEnvironmentVariable("SEEPATH_SMTP_PASSWORD");
            var envHost = Environment.GetEnvironmentVariable("SEEPATH_SMTP_HOST");
            var envPort = Environment.GetEnvironmentVariable("SEEPATH_SMTP_PORT");
            var envSsl = Environment.GetEnvironmentVariable("SEEPATH_SMTP_ENABLESSL");

            var cfg = new SmtpConfig();

            if (!string.IsNullOrWhiteSpace(envHost)) cfg.Host = envHost.Trim();
            if (int.TryParse(envPort, out var parsedPort)) cfg.Port = parsedPort;
            if (bool.TryParse(envSsl, out var parsedSsl)) cfg.EnableSsl = parsedSsl;
            if (!string.IsNullOrWhiteSpace(envUser)) cfg.User = envUser.Trim();
            if (!string.IsNullOrWhiteSpace(envPass)) cfg.Password = NormalizePassword(envPass);

            if (!string.IsNullOrWhiteSpace(cfg.User) && !string.IsNullOrWhiteSpace(cfg.Password))
                return cfg;

            // 2) Local JSON config next to the .exe (ignored by git)
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smtpconfig.json");
            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                var fileCfg = JsonSerializer.Deserialize<SmtpConfig>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (fileCfg != null)
                {
                    fileCfg.Host = string.IsNullOrWhiteSpace(fileCfg.Host) ? "smtp.gmail.com" : fileCfg.Host.Trim();
                    fileCfg.Port = fileCfg.Port == 0 ? 587 : fileCfg.Port;
                    fileCfg.User = (fileCfg.User ?? "").Trim();
                    fileCfg.Password = NormalizePassword(fileCfg.Password);

                    if (!string.IsNullOrWhiteSpace(fileCfg.User) && !string.IsNullOrWhiteSpace(fileCfg.Password))
                        return fileCfg;
                }
            }

            throw new InvalidOperationException(
                "SMTP is not configured. Set SEEPATH_SMTP_USER and SEEPATH_SMTP_PASSWORD, or create smtpconfig.json next to the application. See smtpconfig.example.json for the format.");
        }

        private static string NormalizePassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "";

            // Gmail app passwords are sometimes copied with spaces; remove them.
            return password.Replace(" ", "").Trim();
        }
    }
}
