using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Infrastructure.JWT;
using Events_WEB_APP.Infrastructure.PasswordHashers;
using Events_WEB_APP.Persistence.Contracts.Auth;
using Events_WEB_APP.Persistence.UnitsOfWork;
using Microsoft.IdentityModel.Tokens;

namespace Events_WEB_APP.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTProvider _jwtProvider;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            IJWTProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
        }

        public async Task RegisterAsync(string userName, string email, string password)
        {
            var role = await _unitOfWork.Roles.FindAsync(r => r.Name == "User");
            var hashedPassword = _passwordHasher.Generate(password);
            var user = new User { 
                UserName = userName,
                Email = email,
                PasswordHash = hashedPassword,
                RoleId = role.Id 
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found!");
            }
            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result == false) 
            {
                throw new Exception("Failed to login!");
            }
            user.Role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);
            var token = _jwtProvider.GenerateToken(user);
            user.RefreshToken = _jwtProvider.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return new AuthResponse
            (
                _jwtProvider.GenerateToken(user),
                user.RefreshToken
            );
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            return user ?? throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        public async Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = _jwtProvider.GetPrincipalFromToken(accessToken);
            var userId = Guid.Parse(principal.FindFirst("userId").Value);

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var rawToken = Uri.UnescapeDataString(refreshToken);
            if (user == null ||
                user.RefreshToken != rawToken ||
                user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
            user.Role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);
            user.RefreshToken = _jwtProvider.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            return new AuthResponse
            (_jwtProvider.GenerateToken(user), user.RefreshToken);
        }
    }
}
