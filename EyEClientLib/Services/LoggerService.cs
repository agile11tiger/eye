using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EyEClientLib.Services
{
    public class LoggerService
    {
        public void Log(
        Exception exception,
        LogLevel logLevel = LogLevel.Error,
        [CallerMemberName] string memberName = default,
        [CallerFilePath] string sourceFilePath = default,
        [CallerLineNumber] int sourceLineNumber = default)
        {
            Log(null, exception, logLevel, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Log(
            string message,
            Exception exception = default,
            LogLevel logLevel = LogLevel.Error,
            [CallerMemberName] string memberName = default,
            [CallerFilePath] string sourceFilePath = default,
            [CallerLineNumber] int sourceLineNumber = default)
        {
            Console.WriteLine($"" +
                $"{logLevel}. " +
                $"{DateTime.UtcNow}. " +
                $"{sourceFilePath}. " +
                $"{memberName}. " +
                $"{sourceLineNumber}.\r\n" +
                $"{message}\r\n" +
                $"{exception}");
        }
    }
}
