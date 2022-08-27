using Business.Abstract;
using Business.Constants;
using Core.Aspects.PostSharp;
using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;

namespace Business.Concrete
{
    [ProviderAspect]
    public class AuthManager : IAuthService
    {
        readonly IUserService _userService;
        readonly ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Login(LoginDto loginDto)
        {
            var result = _userService.GetByEMail(loginDto.Email);
            if (result.Data == null)
                return new ErrorDataResult<User>();
            bool isPasswordTrue = HashingHelper.VerifyPasswordHash(loginDto.Password, result.Data.PasswordSalt, result.Data.PasswordHash);
            if (!isPasswordTrue)
                return new ErrorDataResult<User>();
            return new SuccessDataResult<User>(result.Data);
        }

        public IDataResult<User> Register(RegisterDto registerDto)
        {
            var result = BusinessRules.Run(UserExists(registerDto.Email));
            if (!result.Success)
                return new ErrorDataResult<User>(result.Message);

            HashingHelper.CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByEMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var token = _tokenHelper.CreateToken(user, _userService.GetClaims(user).Data);
            return new SuccessDataResult<AccessToken>(token);
        }
    }
}
