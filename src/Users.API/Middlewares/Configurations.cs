using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Users.Application.Authentication;
using Users.Application.Command.CreateUser;
using Users.Domain.Repositories;
using Users.Infrastructure.Authentication;
using Users.Infrastructure.ExternalServices.Messages;
using Users.Infrastructure.Messages;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;

namespace Users.API.Middlewares
{
    public static class Configurations
    {
        public static void AddCustomMiddlewares(this WebApplicationBuilder builder)
        {
            builder.RegisterServicesDependencies();
            builder.SwaggerDocumentation();
            builder.DbContextConfiguration();
            builder.SecurityMiddleware();
        }

        public static void RegisterServicesDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.Configure<SecurityKey>(builder.Configuration.GetSection(nameof(SecurityKey)));

            builder.Services.AddHostedService<UserRoleIntegrationService>();

            builder.Services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<CreateUserCommand>());

            builder.Services.Configure<BusSettings>(builder.Configuration.GetSection(nameof(BusSettings)));
        }

        public static void DbContextConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty));
        }

        public static void SecurityMiddleware(this WebApplicationBuilder builder)
        {
            var jwtDataSection = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JsonWebTokenData>(jwtDataSection);

            var jwt = jwtDataSection.Get<JsonWebTokenData>();
            var key = Encoding.ASCII.GetBytes(jwtDataSection.Key);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt?.Issuer,
                    ValidAudience = jwt?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(jwt?.Key ?? string.Empty))
                };
            });

            builder.Services.AddAuthorization();
        }
    }
}
