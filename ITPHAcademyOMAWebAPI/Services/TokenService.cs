using ITPHAcademyOMAWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITPHAcademyOMAWebAPI.Services
{
    public class TokenService : ITokenService
    {
        private const double EXPIRY_DURATION_MINUTES = 30;
        private readonly ITPHAcademyOMAContext _context;
        private IConfiguration _config;
        public TokenService(ITPHAcademyOMAContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string BuildToken(User user)
        {
            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("roleid",user.RoleId.ToString()),
        new Claim("userid",user.Id.ToString()),
                 };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddHours(1),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Issuer"],
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);

            }
            catch
            {
                return false;
            }
            return true;
        }
        public string GetToken(string token)
        {
            return token.Contains(" ") ? token.Split(' ')[1] : token;
        }

        public User GetUser(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            int userId = Int32.Parse(jwt.Claims.First(c => c.Type == "userid").Value);
            User? user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            return user;
        }
    }
}
