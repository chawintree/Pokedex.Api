using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api.Domain.IoC;

namespace Pokedex.Api
{
    partial class Program
    {
        static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();
            builder.Services.AddServices(builder.Configuration);

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseHttpLogging();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}