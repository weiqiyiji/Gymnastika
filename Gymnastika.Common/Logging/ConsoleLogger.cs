using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Gymnastika.Common.Logging
{
    public class ConsoleLogger : ILogger
    {
        #region ILogger Members

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        public void Log(LogLevel level, string category, string format, params object[] args)
        {
            Debug.WriteLine(
                "[{0}] {1}: ({2}) {3}", 
                level.ToString(), 
                category, 
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                args == null ? format : string.Format(format, args));
        }

        #endregion
    }
}
