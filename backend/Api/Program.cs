using Api;
using Api.Domain.Repositories;
using Api.Domain.Services;
using Api.Infrastructure;
using Api.Infrastructure.Repositories;
using NHibernate;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddLeapEventsServices(builder.Configuration);

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

