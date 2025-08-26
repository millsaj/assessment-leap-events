using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Api.Infrastructure.Mappings;

namespace Api.Infrastructure;

public static class NHibernateSessionFactory
{
    public static ISessionFactory CreateSessionFactory(string connectionString)
    {
        return Fluently.Configure()
            .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
            .Mappings(m =>
            {
                m.FluentMappings.Add<EventMap>();
                m.FluentMappings.Add<TicketSaleMap>();
            })
            .BuildSessionFactory();
    }
}
