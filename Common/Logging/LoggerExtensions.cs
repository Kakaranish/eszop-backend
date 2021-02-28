using System;
using Microsoft.Extensions.Logging;

namespace Common.Logging
{
    public static class LoggerExtensions
    {
        public static void LogWithProps<TLogger>(this ILogger<TLogger> logger, LogLevel logLevel,
            string message, params string[] props)
        {
            logger.Log(logLevel, $"{message} | Props=[{string.Join(";", props)}]");
        }

        public static void LogWithProps<TLogger>(this ILogger<TLogger> logger, LogLevel logLevel, Exception ex,
            string message, params string[] props)
        {
            logger.Log(logLevel, ex, $"{message} | Props=[{string.Join(";", props)}]");
        }
    }
}
