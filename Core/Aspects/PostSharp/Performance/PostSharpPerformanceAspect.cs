using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Diagnostics;
using System.Reflection;

namespace Core.Aspects.PostSharp.Performance
{
    [PSerializable]
    public class PostSharpPerformanceAspect : OnMethodBoundaryAspect
    {
        [PNonSerialized]
        Stopwatch _stopwatch;

        int _interval;
        public PostSharpPerformanceAspect(int interval)
        {
            _interval = interval;
        }
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            AspectPriority = 0;
        }
        public override void RuntimeInitialize(MethodBase method)
        {
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            _stopwatch.Start();
        }
        public override void OnExit(MethodExecutionArgs args)
        {
            if (_stopwatch.Elapsed.TotalMilliseconds > _interval)
                Debug.WriteLine($"Performance : {args.Method.DeclaringType.FullName}.{args.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            _stopwatch.Reset();
        }
    }
}
