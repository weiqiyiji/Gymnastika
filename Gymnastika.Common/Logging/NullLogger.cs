using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.Logging
{
    public class NullLogger : ILogger
    {
        private static ILogger Logger;

        static NullLogger()
        {
            Logger = new NullLogger();
        }

        public static ILogger Instance { get { return Logger; } }

        #region ILogger Members

        public bool IsEnabled(LogLevel level)
        {
            return false;
        }

        public void Log(LogLevel level, string category, string format, params object[] args)
        {
        }

        #endregion
    }
}
