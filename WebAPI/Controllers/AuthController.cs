using Business.Abstract;
using Core.Entities.Concrete.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var loginResult = _authService.Login(loginDto);
            if (!loginResult.Success)
                return BadRequest(loginResult);
            var tokenResult = _authService.CreateAccessToken(loginResult.Data);
            if (!tokenResult.Success)
                return BadRequest(tokenResult);
            return Ok(tokenResult);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var registerResult = _authService.Register(registerDto);
            if (!registerResult.Success)
                return BadRequest(registerResult);
            var tokenResult = _authService.CreateAccessToken(registerResult.Data);
            if (!tokenResult.Success)
                return BadRequest(tokenResult);
            return Ok(tokenResult);
        }
    }
}
