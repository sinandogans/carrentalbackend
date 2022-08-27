using Business.Abstract;
using Core.Aspects.PostSharp;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrete
{
    [ProviderAspect]
    public class OperationClaimManager : IOperationClaimService
    {
        readonly IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        public IResult Add(OperationClaim operationClaim)
        {
            _operationClaimDal.Add(operationClaim);
            return new SuccessResult();
        }

        public IResult Delete(OperationClaim operationClaim)
        {
            _operationClaimDal.Delete(operationClaim);
            return new SuccessResult();
        }

        public IDataResult<List<OperationClaim>> GetAll()
        {
            var data = _operationClaimDal.GetList();
            return new SuccessDataResult<List<OperationClaim>>(data);
        }

        public IDataResult<OperationClaim> GetById(int operationClaimId)
        {
            var data = _operationClaimDal.Get(c => c.Id == operationClaimId);
            return new SuccessDataResult<OperationClaim>(data);
        }

        public IResult Update(OperationClaim operationClaim)
        {
            _operationClaimDal.Update(operationClaim);
            return new SuccessResult();
        }
    }
}
