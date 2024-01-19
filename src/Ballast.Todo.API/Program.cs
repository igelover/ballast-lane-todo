using System.Diagnostics.CodeAnalysis;
using Ballast.Todo.API.Middleware;
using Ballast.Todo.Application;
using Ballast.Todo.Infrastructure;

namespace Ballast.Todo.API
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddOpenApiDocumentation();

            // Add custom services.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            //builder.Services.Configure<GbmChallengeSettings>(
            //    builder.Configuration.GetSection(nameof(GbmChallengeSettings))
            //);

            var app = builder.Build();

            app.UseMiddleware<ExceptionResponseMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseOpenApiDocumentation();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
