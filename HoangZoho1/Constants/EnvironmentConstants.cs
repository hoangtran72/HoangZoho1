using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HoangZoho1.Constants
{
    internal static class EnvironmentConstants
    {
        public static bool LoadFallbackAndValidate(string contentRootPath)
        {
            var missing = RequiredEnvironmentVariables.Names
                .Where(name => string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)))
                .ToArray();

            var fallbackUsed = false;
            if (missing.Length > 0 && string.Equals(
                Environment.GetEnvironmentVariable("ALLOW_LOCAL_SECRET_FALLBACK"),
                "true",
                StringComparison.OrdinalIgnoreCase))
            {
                var configuredPath = Environment.GetEnvironmentVariable("LOCAL_SECRET_FILE_PATH");
                var fallbackPath = string.IsNullOrWhiteSpace(configuredPath)
                    ? Path.Combine(contentRootPath, ".env.azure.local")
                    : configuredPath;

                LoadFallbackFile(fallbackPath);
                fallbackUsed = true;
                missing = RequiredEnvironmentVariables.Names
                    .Where(name => string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)))
                    .ToArray();
            }

            if (missing.Length > 0)
            {
                throw new InvalidOperationException(
                    "Missing required environment variables: " + string.Join(", ", missing));
            }

            return fallbackUsed;
        }

        public static string Get(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Missing required environment variable: {name}");
            return value;
        }

        private static void LoadFallbackFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The configured local secret fallback file was not found.", path);

            foreach (var rawLine in File.ReadLines(path))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                var separator = line.IndexOf('=');
                if (separator <= 0)
                    continue;

                var name = line.Substring(0, separator).Trim();
                var rawValue = line.Substring(separator + 1).Trim();
                var value = rawValue.StartsWith("\"")
                    ? JsonConvert.DeserializeObject<string>(rawValue)
                    : rawValue;

                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)))
                    Environment.SetEnvironmentVariable(name, value);
            }
        }
    }
}
