using Core.Aspects.PostSharp.Logging;
using Core.CrossCuttingConcerns.Logging.NLog;
using PostSharp.Aspects;

namespace Core.Aspects.PostSharp
{
    public class ProviderAspect : TypeLevelAspect, IAspectProvider
    {
        public IEnumerable<AspectInstance> ProvideAspects(object targetElement)
        {
            Type type = (Type)targetElement;
            List<AspectInstance> instances = new List<AspectInstance>();
            var methodInfos = type.GetMethods();
            foreach (var methodInfo in methodInfos)
            {
                instances.Add(new AspectInstance(methodInfo,
                    new PostSharpLogAspect(typeof(NLogFileLogger))));
                instances.Add(new AspectInstance(methodInfo,
                    new PostSharpLogAspect(typeof(NLogDatabaseLogger))));
                //instances.Add(new AspectInstance(methodInfo,
                //    new PostSharpPerformanceAspect(5)));
            }

            return instances.AsEnumerable();
        }
    }
}