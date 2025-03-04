using Events_WEB_APP.Infrastructure.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Events_WEB_APP.API.Extentions
{
    public static class ApiExtentions
    {
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
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["definitely-not-jwt-token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.RequireRole("User");
                });
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireRole("Admin");
                });
            });
        }
    }
}
