using Business.Abstract;
using Business.Constants;
using Core.Aspects.PostSharp;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrete
{
    [ProviderAspect]
    public class UserManager : IUserService
    {
        readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult(Messages.UserAdded);
        }

        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult(Messages.UserDeleted);
        }

        public IDataResult<List<User>> GetAll()
        {
            var data = _userDal.GetList();
            return new SuccessDataResult<List<User>>(Messages.AllUsersGet, data);
        }

        public IDataResult<User> GetByEMail(string email)
        {
            var data = _userDal.Get(u=>u.Email == email);
            return new SuccessDataResult<User>(data);
        }

        public IDataResult<User> GetById(int userId)
        {
            var data = _userDal.Get(u => u.Id == userId);
            return new SuccessDataResult<User>(Messages.UserGetById, data);
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            var data = _userDal.GetOperationClaims(user);
            return new SuccessDataResult<List<OperationClaim>>(data);
        }

        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdated);
        }
    }
}
