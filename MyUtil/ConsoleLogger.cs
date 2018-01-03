using System;

namespace MyUtil
{
    public class ConsoleLogger : ILogger
    {
        private void WriteMessage(string message, string level)
        {
            System.Diagnostics.Debug.WriteLine($"[{level.ToUpper()}]\t{new DateTime().ToShortTimeString()}\t{message}");
        }

        public void Debug(string message)
        {
            WriteMessage(message, "Debug");
        }

        public void Error(string message)
        {
            WriteMessage(message, "error");
        }

        public void Info(string message)
        {
            WriteMessage(message, "info");
        }

        public void Trace(string message)
        {
            WriteMessage(message, "trace");
        }

        public void Warn(string message)
        {
            WriteMessage(message, "warn");
        }
    }
}
