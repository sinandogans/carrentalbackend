using Business.Abstract;
using Core.Aspects.PostSharp;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrete
{
    [ProviderAspect]
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        public IResult Add(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult();
        }

        public IResult Delete(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Delete(userOperationClaim);
            return new SuccessResult();
        }

        public IDataResult<List<UserOperationClaim>> GetAll()
        {
            var data = _userOperationClaimDal.GetList();
            return new SuccessDataResult<List<UserOperationClaim>>(data);
        }

        public IDataResult<UserOperationClaim> GetById(int userOperationClaimId)
        {
            var data = _userOperationClaimDal.Get(c => c.Id == userOperationClaimId);
            return new SuccessDataResult<UserOperationClaim>(data);
        }

        public IResult Update(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Update(userOperationClaim);
            return new SuccessResult();
        }
    }
}
