using Events_WEB_APP.Core.Entities;
using System.Security.Claims;

namespace Events_WEB_APP.Infrastructure.JWT
{
    public interface IJWTProvider
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}