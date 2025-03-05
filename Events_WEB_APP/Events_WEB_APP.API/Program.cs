using Events_WEB_APP.Persistence;
using Events_WEB_APP.Application;
using Events_WEB_APP.Infrastructure;
using Events_WEB_APP.Infrastructure.JWT;
using Events_WEB_APP.API.Mapping;
using Events_WEB_APP.API.Extentions;
using Events_WEB_APP.API.Middlewares;
using Events_WEB_APP.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection(nameof(JWTOptions)));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(CategoryProfile));
builder.Services.AddAutoMapper(typeof(RoleProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(EventProfile));
builder.Services.AddAutoMapper(typeof(ParticipantProfile));
builder.Services.AddApiAuth(builder.Configuration);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
var app = builder.Build();

// Configure the HTTP request pipeline.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EventsAppDbContext>();
    db.Database.Migrate();
}
app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
