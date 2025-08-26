using Api.Domain.Repositories;
using Api.Domain.Services;
using Api.Infrastructure;
using Api.Infrastructure.Repositories;
using NHibernate;

namespace Api;

public static class ServiceRegistration
{
    public static void AddLeapEventsServices(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string 'DefaultConnection' not found.");
        var sessionFactory = NHibernateSessionFactory.CreateSessionFactory(connStr);

        services.AddSingleton(sessionFactory);
        
        services.AddScoped(factory => sessionFactory.OpenSession());
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ITicketService, TicketService>();
    }
}
