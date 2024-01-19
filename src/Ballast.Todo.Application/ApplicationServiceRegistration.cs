using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ballast.Todo.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITodoService, TodoService>();

            return services;
        }
    }
}
