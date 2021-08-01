using System;

namespace Common.Utilities.Helpers
{
    public static class EnvironmentHelpers
    {
        public static bool IsSeedingDatabase()
        {
            var str = Environment.GetEnvironmentVariable("ESZOP_DB_SEED");
            return !string.IsNullOrWhiteSpace(str)
                   && bool.TryParse(str, out var isSeeding) && isSeeding;
        }

        public static string GetRequiredEnvVariable(string environmentVariableName)
        {
            var envVarValue = Environment.GetEnvironmentVariable(environmentVariableName);
            if (string.IsNullOrWhiteSpace(envVarValue))
                throw new InvalidOperationException($"Environment variable {environmentVariableName} is not set");

            return envVarValue;
        }
    }
}
