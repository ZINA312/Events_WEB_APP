using Events_WEB_APP.Infrastructure.JWT;
using Events_WEB_APP.Infrastructure.PasswordHashers;
using Microsoft.Extensions.DependencyInjection;

namespace Events_WEB_APP.Infrastructure
{
    /// <summary>
    /// Статический класс для регистрации зависимостей в контейнере внедрения зависимостей.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Добавляет инфраструктурные службы в контейнер зависимостей.
        /// </summary>
        /// <param name="services">Коллекция служб для регистрации.</param>
        /// <returns>Обновленная коллекция служб.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJWTProvider, JWTProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
