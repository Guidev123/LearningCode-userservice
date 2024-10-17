using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Users.Application.Authentication;
using Users.Application.Command.CreateUser;
using Users.Domain.Repositories;
using Users.Infrastructure.Authentication;
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

            builder.Services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<CreateUserCommand>());
        }

        public static void DbContextConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty));
        }

        public static void SwaggerDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Lerning Code Enterprise API",
                    Contact = new OpenApiContact() { Name = "Guilherme Nascimento", Email = "guirafaelrn@gmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/license/MIT") }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta forma: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.AddSecurityDefinition("AcessSecretKey", new OpenApiSecurityScheme
                {
                    Description = "Chave de acesso necessária para acessar o sistema",
                    Name = "AcessSecretKey",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "AcessSecretKey"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
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

        }
    }
}
