﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Users.Application.Authentication;
using Users.Application.BackgroundServices;
using Users.Application.Command.CreateUser;
using Users.Application.Extensions;
using Users.Application.MessageBus.Configuration;
using Users.Domain.Repositories;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;

namespace Users.API.Configurations
{
    public static class Configurations
    {
        public const string BUS_SETTINGS = "BusSettings";

        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.RegisterServicesDependencies();
            builder.SwaggerDocumentation();
            builder.DbContextConfiguration();
            builder.SecurityMiddleware();
            builder.AddSubscribers();
        }

        public static void RegisterServicesDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.Configure<SecurityKey>(builder.Configuration.GetSection(nameof(SecurityKey)));
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreateUserCommand>());
            builder.Services.Configure<BusSettingsConfiguration>(builder.Configuration.GetSection(nameof(BusSettingsConfiguration)));
        }

        public static void AddSubscribers(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<UpdateUserRoleService>();
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
            var key = Encoding.UTF8.GetBytes(jwtDataSection.Key);

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
