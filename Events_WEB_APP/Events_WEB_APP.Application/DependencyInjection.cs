using Events_WEB_APP.Application.Services.CategoryService;
using Events_WEB_APP.Application.Services.EventService;
using Events_WEB_APP.Application.Services.ParticipantService;
using Events_WEB_APP.Application.Services.RoleService;
using Events_WEB_APP.Application.Services.UserService;
using Events_WEB_APP.Application.Validators;
using Events_WEB_APP.Core.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Events_WEB_APP.Application
{
    /// <summary>
    /// Класс для настройки внедрения зависимостей.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Добавляет сервисы приложения в контейнер служб.
        /// </summary>
        /// <param name="services">Контейнер служб для добавления зависимостей.</param>
        /// <returns>Обновленный контейнер служб.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IValidator<Category>, CategoryValidator>();
            services.AddScoped<IValidator<Event>, EventValidator>();
            services.AddScoped<IValidator<Participant>, ParticipantValidator>();
            services.AddScoped<IValidator<Role>, RoleValidator>();
            services.AddScoped<IValidator<User>, UserValidator>();

            return services;
        }
    }
}
