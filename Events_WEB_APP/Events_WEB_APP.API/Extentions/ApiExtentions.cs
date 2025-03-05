using Events_WEB_APP.Infrastructure.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Events_WEB_APP.API.Extentions
{
    public static class ApiExtentions
    {
        /// <summary>
        /// Добавляет аутентификацию и авторизацию API.
        /// </summary>
        /// <param name="services">Коллекция сервисов для внедрения зависимостей.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        public static void AddApiAuth(
             this IServiceCollection services,
             IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JWTOptions").Get<JWTOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false, // Не проверять издателя токена
                        ValidateAudience = false, // Не проверять аудиторию токена
                        ValidateLifetime = true, // Проверять срок действия токена
                        ValidateIssuerSigningKey = true, // Проверять ключ подписи токена
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey)) // Ключ подписи
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Получаем токен из куки
                            context.Token = context.Request.Cookies["definitely-not-jwt-token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                // Политика для пользователей
                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.RequireRole("User");
                });
                // Политика для администраторов
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireRole("Admin");
                });
            });
        }
    }
}
