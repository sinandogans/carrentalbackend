using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Models;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using System.Text.Json;

namespace Core.Aspects.PostSharp.Logging
{
    [PSerializable]
    public class PostSharpLogAspect : OnMethodBoundaryAspect
    {
        [PNonSerialized] private ILoggerManager _loggerManager;
        private Type _type;

        public PostSharpLogAspect(Type loggerManager)
        {
            _type = loggerManager;
        }
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            AspectPriority = 1;
        }
        public override void RuntimeInitialize(MethodBase method)
        {
            if (_type.GetInterface("ILoggerManager") == null && _type.BaseType?.GetInterface("ILoggerManager") == null)
                throw new Exception("Wrong logger type.");
            _loggerManager = (ILoggerManager)Activator.CreateInstance(_type);
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            _loggerManager.LogInfo(ConvertToJson(GetLogDetails(args)));
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            _loggerManager.LogInfo(ConvertToJson(GetLogDetailsSuccess(args)));
        }

        public override void OnException(MethodExecutionArgs args)
        {
            _loggerManager.LogError(ConvertToJson(GetLogDetailsException(args, args.Exception)));
        }

        private LogDetail GetLogDetails(MethodExecutionArgs args)
        {
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < args.Arguments.Count; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = args.Method.GetParameters()[i].Name,
                    Value = args.Arguments[i],
                    Type = args.Arguments[i].GetType().Name
                });
            }

            var logDetail = new LogDetail
            {
                LogParameters = logParameters,
                MethodName = args.Method.Name,
            };

            return logDetail;
        }

        private LogDetailException GetLogDetailsException(MethodExecutionArgs args, Exception e)
        {
            var logDetail = GetLogDetails(args);

            var logDetailException = new LogDetailException
            {
                LogParameters = logDetail.LogParameters,
                MethodName = logDetail.MethodName,
                ErrorMessage = e.Message
            };

            return logDetailException;
        }

        private LogDetailSuccess GetLogDetailsSuccess(MethodExecutionArgs args)
        {
            var logDetail = GetLogDetails(args);

            var logDetailSuccess = new LogDetailSuccess
            {
                LogParameters = logDetail.LogParameters,
                MethodName = logDetail.MethodName,
                ReturnValue = args.ReturnValue
            };
            return logDetailSuccess;
        }

        private string ConvertToJson(object o)
        {
            return JsonSerializer.Serialize(o);
        }
    }
}