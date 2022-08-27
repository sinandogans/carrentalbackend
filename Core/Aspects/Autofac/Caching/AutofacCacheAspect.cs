using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Text;

namespace Core.Aspects.Autofac.Caching
{
    public class AutofacCacheAspect : MethodInterception
    {
        readonly int _duration;
        readonly ICacheManager _cacheManager;
        Type _returnType;

        public AutofacCacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
            Priority = 4;
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();

            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            if (_cacheManager.IsAdd(key))
            {
                var cachedValue = _cacheManager.Get(key);
                if (cachedValue.GetType() != _returnType)
                    cachedValue = JsonSerializer.DeserializeFromString(cachedValue.ToString(), _returnType);

                invocation.ReturnValue = cachedValue;
                return;
            }
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
            _returnType = invocation.ReturnValue.GetType();
        }
    }
}
