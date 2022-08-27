using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Messages;
using FluentValidation;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;

namespace Core.Aspects.PostSharp.Validation
{
    [PSerializable]
    public class PostSharpValidationAspect : OnMethodBoundaryAspect
    {
        private Type _validatorType;

        public PostSharpValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new SystemException(ValidationMessages.WrongType);
            }
            _validatorType = validatorType;
        }

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            AspectPriority = 5;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = args.Arguments.Where(t => t.GetType() == entityType);
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}

