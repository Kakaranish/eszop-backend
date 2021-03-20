using System;

namespace Common.Helpers
{
    public static class EnvironmentHelpers
    {
        public static bool IsSeedingDatabase()
        {
            var str = Environment.GetEnvironmentVariable("ESZOP_DB_SEED");
            return !string.IsNullOrWhiteSpace(str)
                   && bool.TryParse(str, out var isSeeding) && isSeeding;
        }
    }
}
