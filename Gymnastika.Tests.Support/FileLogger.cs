using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Logging;
using System.IO;
using System.Threading;

namespace Gymnastika.Tests.Support
{
    public class FileLogger : ILogger
    {
        private const string LogFileName = "logs.txt";

        #region ILogger Members

        public bool IsEnabled(LogLevel level)
        {
            return true;
        }

        public void Log(LogLevel level, string category, string format, params object[] args)
        {
            string directory = Directory.GetCurrentDirectory();
            string logFilePath = Path.Combine(directory, LogFileName);

            if (!File.Exists(logFilePath))
            {
                Stream stream = File.Create(logFilePath);
                stream.Close();
            }

            StreamWriter writer = new StreamWriter(new FileStream(logFilePath, FileMode.Append));
            writer.WriteLine("[{0}] [t:{4}] {1} {2}: {3}",
                    level.ToString(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    category ?? "Default",
                    args == null ? format : string.Format(format, args),
                    Thread.CurrentThread.ManagedThreadId);
            writer.Close();
        }

        #endregion
    }
}
