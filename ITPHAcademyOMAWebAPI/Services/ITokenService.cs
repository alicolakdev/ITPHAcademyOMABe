using ITPHAcademyOMAWebAPI.Models;
using Microsoft.Extensions.Primitives;

namespace ITPHAcademyOMAWebAPI.Services
{
    public interface ITokenService
    {
        string BuildToken(User user);
        bool ValidateToken(string token);
        string GetToken(string token);
        User GetUser(string token);
    }
}
