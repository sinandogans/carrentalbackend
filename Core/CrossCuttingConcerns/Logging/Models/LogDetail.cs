namespace Core.CrossCuttingConcerns.Logging.Models
{
    public class LogDetail
    {
        public string MethodName { get; set; }
        public List<LogParameter> LogParameters { get; set; }
    }
}
