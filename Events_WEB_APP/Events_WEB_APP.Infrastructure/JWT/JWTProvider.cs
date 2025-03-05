using Events_WEB_APP.Core.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Events_WEB_APP.Infrastructure.JWT
{
    /// <summary>
    /// Реализация интерфейса <see cref="IJWTProvider"/> для работы с JWT (JSON Web Tokens).
    /// </summary>
    public class JWTProvider : IJWTProvider
    {
        private readonly JWTOptions _options;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="JWTProvider"/>.
        /// </summary>
        /// <param name="options">Настройки JWT.</param>
        public JWTProvider(IOptions<JWTOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Генерирует JWT для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого генерируется токен.</param>
        /// <returns>Сгенерированный токен.</returns>
        public string GenerateToken(User user)
        {
            var claims = new List<Claim> 
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        /// <summary>
        /// Генерирует токен обновления.
        /// </summary>
        /// <returns>Сгенерированный токен обновления.</returns>
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        /// <summary>
        /// Извлекает объект <see cref="ClaimsPrincipal"/> из токена.
        /// </summary>
        /// <param name="token">JWT, из которого будет извлечен объект.</param>
        /// <returns>Объект <see cref="ClaimsPrincipal"/> из токена.</returns>
        /// <exception cref="SecurityTokenException">Выбрасывается, если токен недействителен.</exception>
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                }, out _);

                return principal;
            }
            catch
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
    }
}
