using Events_WEB_APP.Persistence.Data;
using Events_WEB_APP.Persistence.Repositories.CategoryRepository;
using Events_WEB_APP.Persistence.Repositories.EventRepository;
using Events_WEB_APP.Persistence.Repositories.ParticipantRepository;
using Events_WEB_APP.Persistence.Repositories.RoleRepository;
using Events_WEB_APP.Persistence.Repositories.UserRepository;
using Events_WEB_APP.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events_WEB_APP.Persistence
{
    /// <summary>
    /// Класс для настройки зависимостей в приложении.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Регистрирует сервисы для работы с базой данных и репозиториями.
        /// </summary>
        /// <param name="services">Коллекция сервисов для регистрации.</param>
        /// <param name="configuration">Конфигурация приложения для получения строк подключения.</param>
        /// <returns>Обновленная коллекция сервисов.</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventsAppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("EventsDatabase"));
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
