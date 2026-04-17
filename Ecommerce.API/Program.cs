using Ecommerce.DAL;
using Ecommerce.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;

namespace Ecommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDataAccess(builder.Configuration);
            builder.Services.AddBLLServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                DatabaseSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
