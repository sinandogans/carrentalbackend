using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(RegisterDto registerDto);
        IDataResult<User> Login(LoginDto loginDto);
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccessToken(User user);

    }
}
