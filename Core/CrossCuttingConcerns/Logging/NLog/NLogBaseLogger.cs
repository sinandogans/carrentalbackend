using NLog;

namespace Core.CrossCuttingConcerns.Logging.NLog
{
    public abstract class NLogBaseLogger : ILoggerManager
    {
        private readonly ILogger _logger;
        public NLogBaseLogger(Logger logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
    }
}
