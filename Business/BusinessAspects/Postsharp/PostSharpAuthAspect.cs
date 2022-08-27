using Business.Constants;
using Core.Extensions;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;

namespace Business.BusinessAspects.Postsharp
{
    [PSerializable]
    public class PostSharpAuthAspect : OnMethodBoundaryAspect
    {
        private string[] _roles;
        [PNonSerialized]
        private IHttpContextAccessor _httpContextAccessor;

        public PostSharpAuthAspect(string roles)
        {
            _roles = roles.Split(",");
        }

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            AspectPriority = 2;
        }
        public override void RuntimeInitialize(MethodBase method)
        {
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                    return;
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
