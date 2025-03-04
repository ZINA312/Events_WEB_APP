using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Auth;

namespace Events_WEB_APP.Application.Services.UserService
{
    public interface IUserService
    {
        Task RegisterAsync(string userName, string email,
            string password);
        Task<AuthResponse> LoginAsync(string email, string password);

        Task<User> GetUserByIdAsync(Guid userId);
        Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}
