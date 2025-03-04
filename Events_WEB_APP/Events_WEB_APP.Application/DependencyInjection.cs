using Events_WEB_APP.Application.Services.CategoryService;
using Events_WEB_APP.Application.Services.EventService;
using Events_WEB_APP.Application.Services.ParticipantService;
using Events_WEB_APP.Application.Services.RoleService;
using Events_WEB_APP.Application.Services.UserService;
using Microsoft.Extensions.DependencyInjection;

namespace Events_WEB_APP.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }
    }
}
