using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Infrastructure.JWT;
using Events_WEB_APP.Infrastructure.PasswordHashers;
using Events_WEB_APP.Persistence.Contracts.Auth;
using Events_WEB_APP.Persistence.UnitsOfWork;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Events_WEB_APP.Application.Services.UserService
{
    /// <summary>
    /// Сервис для управления пользователями.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTProvider _jwtProvider;
        private readonly IValidator<User> _validator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserService"/>.
        /// </summary>
        /// <param name="unitOfWork">Единица работы для доступа к репозиториям.</param>
        /// <param name="passwordHasher">Хешер паролей для обработки паролей.</param>
        /// <param name="jwtProvider">Поставщик JWT для генерации токенов.</param>
        /// <param name="validator">Валидатор для проверки пользователей.</param>
        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            IJWTProvider jwtProvider, IValidator<User> validator)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _validator = validator;
        }

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
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
            await ValidateUserAsync(user);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Выполняет вход пользователя.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Ответ с информацией о аутентификации.</returns>
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
            await ValidateUserAsync(user);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return new AuthResponse
            (
                _jwtProvider.GenerateToken(user),
                user.RefreshToken
            );
        }

        /// <summary>
        /// Удаляет пользователя по идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для удаления.</param>
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Найденный пользователь.</returns>
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            return user ?? throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        /// <returns>Список всех пользователей.</returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        /// <summary>
        /// Обновляет токены доступа и обновления.
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <returns>Ответ с обновленными токенами.</returns>
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
            await ValidateUserAsync(user);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            return new AuthResponse
            (_jwtProvider.GenerateToken(user), user.RefreshToken);
        }

        private async Task ValidateUserAsync(User user)
        {
            var validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }
    }
}
