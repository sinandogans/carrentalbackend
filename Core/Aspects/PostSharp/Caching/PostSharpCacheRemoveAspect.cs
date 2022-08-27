using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;

namespace Core.Aspects.PostSharp.Caching
{
    [PSerializable]
    public class PostSharpCacheRemoveAspect : OnMethodBoundaryAspect
    {
        string _pattern;
        [PNonSerialized]
        ICacheManager _cacheManager;
        public PostSharpCacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
        }
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            AspectPriority = 6;
        }
        public override void RuntimeInitialize(MethodBase method)
        {
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }


        public override void OnSuccess(MethodExecutionArgs args)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
