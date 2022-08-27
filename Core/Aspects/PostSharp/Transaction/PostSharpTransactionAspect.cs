using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using System.Transactions;

namespace Core.Aspects.PostSharp.Transaction
{
    [PSerializable]
    public class PostSharpTransactionAspect : OnMethodBoundaryAspect
    {
        [PNonSerialized]
        TransactionScope _transactionScope;

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            AspectPriority = 3;
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            _transactionScope = new TransactionScope();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            _transactionScope.Dispose();
            throw new System.Exception(args.Exception.Message);
        }
        public override void OnSuccess(MethodExecutionArgs args)
        {
            _transactionScope.Complete();
        }
    }
}
