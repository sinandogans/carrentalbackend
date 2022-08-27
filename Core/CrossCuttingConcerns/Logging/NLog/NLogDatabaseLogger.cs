using NLog;

namespace Core.CrossCuttingConcerns.Logging.NLog
{
    public class NLogDatabaseLogger : NLogBaseLogger
    {
        public NLogDatabaseLogger() : base(LogManager.GetLogger("DatabaseLogger"))
        {
        }
    }
}
