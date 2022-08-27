using NLog;

namespace Core.CrossCuttingConcerns.Logging.NLog
{
    public class NLogFileLogger : NLogBaseLogger
    {
        public NLogFileLogger() : base(LogManager.GetLogger("FileLogger"))
        {
        }
    }
}
