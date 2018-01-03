using NLog;
using System;

namespace MyUtil
{
    public class NLogLogger : ILogger
    {
        private readonly Logger _logger;

        public NLogLogger(string className)
        {
            _logger = LogManager.GetLogger(className);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}
