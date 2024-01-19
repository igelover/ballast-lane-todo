using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;
using Ballast.Todo.Application.Contracts.Identity;
using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Domain;
using Ballast.Todo.Infrastructure.Database;
using Ballast.Todo.Infrastructure.Database.Repositories;
using Ballast.Todo.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Ballast.Todo.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterIdGenerator(typeof(string), new StringObjectIdGenerator());

            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IAuthorizationHandler, TokenAuthorizationHandler>();

            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();

            return services;
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var signingKey = configuration["Jwt:Key"];

            services.AddAuthorization();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = audience,
                            ValidIssuer = issuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                            RoleClaimType = ClaimTypes.Role,
                            NameClaimType = ClaimTypes.NameIdentifier
                        };
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy =>
                {
                    policy.RequireRole(Constants.JwtScopeUserRole);
                    policy.AddRequirements(new ValidTokenRequirement());
                });
            });

            services.AddHttpContextAccessor();
        }
    }
}
