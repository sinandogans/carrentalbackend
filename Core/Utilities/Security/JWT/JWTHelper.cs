using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private readonly TokenOptions _tokenOptions;
        private readonly DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

        }

        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            /*
            Yukardakine alternatif olarak kullanılabilir. 
            Helper sınıfı kullanmak yerine helper sınıfın yaptığı şeyi eller yaptık.
            
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            */

            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            /*
            Yukardakine alternatif olarak kullanılabilir. 
            Helper sınıfı kullanmak yerine helper sınıfın yaptığı şeyi eller yaptık.

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            */
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName(user.FirstName + user.LastName);
            claims.AddRoles(operationClaims.Select(c => c.ClaimName).ToArray());

            JwtSecurityToken jwt = new(
                issuer: "sinan",
                audience: _tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials,
                claims: claims
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
    }
}
