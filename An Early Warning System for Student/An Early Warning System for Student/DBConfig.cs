using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text.Json;

namespace An_Early_Warning_System_for_Student
{
    public static class DBConfig
    {
        private const string DefaultConnectionString = "server=localhost;database=earlywarning;uid=root;pwd=;";
        private static string? _connectionString;

        // Reads dbconfig.json from the app folder (same directory as the EXE).
        // If missing/invalid, falls back to the default localhost connection.
        public static string ConnectionString => _connectionString ??= LoadConnectionString();

        private static string LoadConnectionString()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "dbconfig.json");
                if (!File.Exists(path))
                    return DefaultConnectionString;

                var json = File.ReadAllText(path);
                var config = JsonSerializer.Deserialize<DbConfigFile>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (config == null || string.IsNullOrWhiteSpace(config.Server))
                    return DefaultConnectionString;

                var builder = new MySqlConnectionStringBuilder
                {
                    Server = config.Server,
                    Database = string.IsNullOrWhiteSpace(config.Database) ? "earlywarning" : config.Database,
                    UserID = string.IsNullOrWhiteSpace(config.User) ? "root" : config.User,
                    Password = config.Password ?? string.Empty,
                    Port = (uint)(config.Port ?? 3306),
                    // Use a connector-compatible mode; XAMPP typically runs without SSL.
                    SslMode = MySqlSslMode.Preferred,
                };

                return builder.ConnectionString;
            }
            catch
            {
                return DefaultConnectionString;
            }
        }

        private sealed class DbConfigFile
        {
            public string? Server { get; set; }
            public int? Port { get; set; }
            public string? Database { get; set; }
            public string? User { get; set; }
            public string? Password { get; set; }
        }
    }
}

