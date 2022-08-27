using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class AutofacTransactionAspect : MethodInterception
    {
        public AutofacTransactionAspect()
        {
            Priority = 3;
        }
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new())
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception exception)
                {
                    transactionScope.Dispose();
                    throw new System.Exception(exception.Message);
                }
            }
        }
    }
}
