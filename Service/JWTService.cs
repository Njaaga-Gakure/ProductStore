using Microsoft.IdentityModel.Tokens;
using ProductStore.Model;
using ProductStore.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductStore.Service
{
    public class JWTService : IJWT
    {
        public readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateJWTToken(User user)
        {

            var secretKey = _configuration.GetSection("JwtOptions:SecretKey").Value;
            var audience = _configuration.GetSection("JwtOptions:Audience").Value;
            var issuer = _configuration.GetSection("JwtOptions:Issuer").Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Role", user.Role));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));


            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddDays(1),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
