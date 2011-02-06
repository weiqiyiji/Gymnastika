using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Debug, null, message, null);
        }
        public static void Information(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Information, null, message, null);
        }
        public static void Warning(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Warning, null, message, null);
        }
        public static void Error(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Error, null, message, null);
        }
        public static void Fatal(this ILogger logger, string message)
        {
            FilteredLog(logger, LogLevel.Fatal, null, message, null);
        }

        public static void Debug(this ILogger logger, string category, string message)
        {
            FilteredLog(logger, LogLevel.Debug, category, message, null);
        }
        public static void Information(this ILogger logger, string category, string message)
        {
            FilteredLog(logger, LogLevel.Information, category, message, null);
        }
        public static void Warning(this ILogger logger, string category, string message)
        {
            FilteredLog(logger, LogLevel.Warning, category, message, null);
        }
        public static void Error(this ILogger logger, string category, string message)
        {
            FilteredLog(logger, LogLevel.Error, category, message, null);
        }
        public static void Fatal(this ILogger logger, string category, string message)
        {
            FilteredLog(logger, LogLevel.Fatal, category, message, null);
        }

        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Debug, null, format, args);
        }
        public static void Information(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Information, null, format, args);
        }
        public static void Warning(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Warning, null, format, args);
        }
        public static void Error(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Error, null, format, args);
        }
        public static void Fatal(this ILogger logger, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Fatal, null, format, args);
        }

        public static void Debug(this ILogger logger, string category, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Debug, category, format, args);
        }
        public static void Information(this ILogger logger, string category, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Information, category, format, args);
        }
        public static void Warning(this ILogger logger, string category, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Warning, category, format, args);
        }
        public static void Error(this ILogger logger, string category, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Error, category, format, args);
        }
        public static void Fatal(this ILogger logger, string category, string format, params object[] args)
        {
            FilteredLog(logger, LogLevel.Fatal, category, format, args);
        }

        private static void FilteredLog(ILogger logger, LogLevel level, string category, string format, object[] objects)
        {
            if (logger.IsEnabled(level))
            {
                logger.Log(level, category, format, objects);
            }
        }
    }
}
