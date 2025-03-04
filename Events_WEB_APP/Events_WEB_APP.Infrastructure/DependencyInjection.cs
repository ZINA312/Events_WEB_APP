using Events_WEB_APP.Infrastructure.JWT;
using Events_WEB_APP.Infrastructure.PasswordHashers;
using Microsoft.Extensions.DependencyInjection;

namespace Events_WEB_APP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJWTProvider, JWTProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
