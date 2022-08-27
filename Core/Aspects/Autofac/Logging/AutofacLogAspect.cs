using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Models;
using Core.Utilities.Interceptors;
using ServiceStack.Text;

namespace Core.Aspects.Autofac.Logging
{
    public class AutofacLogAspect : MethodInterception
    {
        private readonly ILoggerManager _loggerManager;

        public AutofacLogAspect(Type loggerManager)
        {
            Priority = 1;
            if (loggerManager.GetInterface("ILoggerManager") == null && loggerManager.BaseType?.GetInterface("ILoggerManager") == null)
                throw new Exception("Wrong logger type.");

            _loggerManager = (ILoggerManager)Activator.CreateInstance(loggerManager);
        }

        protected override void OnBefore(IInvocation invocation)
        {

            _loggerManager.LogInfo(ConvertToJson(GetLogDetails(invocation)));
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _loggerManager.LogInfo(ConvertToJson(GetLogDetailsSuccess(invocation)));
        }

        protected override void OnException(IInvocation invocation, Exception exception)
        {
            _loggerManager.LogError(ConvertToJson(GetLogDetailsException(invocation, exception)));
        }

        private LogDetail GetLogDetails(IInvocation invocation)
        {
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value = invocation.Arguments[i],
                    Type = invocation.Arguments[i].GetType().Name
                });
            }
            var logDetail = new LogDetail
            {
                LogParameters = logParameters,
                MethodName = invocation.Method.Name,
            };

            return logDetail;
        }

        private LogDetailException GetLogDetailsException(IInvocation invocation, Exception e)
        {

            var logDetail = GetLogDetails(invocation);

            var logDetailException = new LogDetailException
            {
                LogParameters = logDetail.LogParameters,
                MethodName = logDetail.MethodName,
                ErrorMessage = e.Message
            };

            return logDetailException;
        }

        private LogDetailSuccess GetLogDetailsSuccess(IInvocation invocation)
        {
            var logDetail = GetLogDetails(invocation);

            var logDetailSuccess = new LogDetailSuccess
            {
                LogParameters = logDetail.LogParameters,
                MethodName = logDetail.MethodName,
                ReturnValue = invocation.ReturnValue
            };
            return logDetailSuccess;
        }

        private string ConvertToJson(object o)
        {
            return JsonSerializer.SerializeToString(o);
        }
    }
}
