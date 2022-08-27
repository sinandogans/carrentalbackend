using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Core.Aspects.Autofac.Performance
{
    public class AutofacPerformanceAspect : MethodInterception
    {
        readonly Stopwatch _stopwatch;
        readonly int _interval;
        public AutofacPerformanceAspect(int interval)
        {
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
            _interval = interval;
            Priority = 0;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalMilliseconds > _interval)
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            _stopwatch.Reset();
        }

    }
}
