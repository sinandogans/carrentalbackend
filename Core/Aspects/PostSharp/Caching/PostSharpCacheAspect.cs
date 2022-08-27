using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Serialization;
using ServiceStack.Text;
using System.Reflection;

namespace Core.Aspects.PostSharp.Caching
{
    [PSerializable]
    public class PostSharpCacheAspect : MethodInterceptionAspect
    {
        int _duration;
        string _key;
        Type _returnType;
        ICacheManager _cacheManager;
        bool _isCached = false;

        public PostSharpCacheAspect(int duration = 5)
        {
            _duration = duration;
        }
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            AspectPriority = 4;
        }
        public override void RuntimeInitialize(MethodBase method)
        {
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        //public override void OnEntry(MethodExecutionArgs args)
        //{
        //    var methodName = string.Format($"{args.Method.ReflectedType.FullName}.{args.Method.Name}");
        //    var arguments = args.Arguments.ToList();

        //    _key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
        //    if (_cacheManager.IsAdd(_key))
        //    {
        //        _isCached = true;
        //        var cachedValue = _cacheManager.Get(_key);
        //        if (cachedValue.GetType() != _returnType)
        //            cachedValue = JsonSerializer.DeserializeFromString(cachedValue.ToString(), _returnType);

        //        args.ReturnValue = cachedValue;
        //        return;
        //    }
        //}

        //public override void OnSuccess(MethodExecutionArgs args)
        //{
        //    if (!_isCached)
        //    {
        //        _cacheManager.Add(_key, args.ReturnValue, _duration);
        //        _returnType = args.ReturnValue.GetType();
        //    }

        //}
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var methodName = string.Format($"{args.Method.ReflectedType.FullName}.{args.Method.Name}");
            var arguments = args.Arguments.ToList();

            _key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            if (_cacheManager.IsAdd(_key))
            {
                var cachedValue = _cacheManager.Get(_key);
                if (cachedValue.GetType() != _returnType)
                    cachedValue = JsonSerializer.DeserializeFromString(cachedValue.ToString(), _returnType);

                args.ReturnValue = cachedValue;
                return;
            }

            args.Proceed();
            _cacheManager.Add(_key, args.ReturnValue, _duration);
            _returnType = args.ReturnValue.GetType();
        }
    }
}