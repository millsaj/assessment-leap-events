using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api.Domain.Entities;
using Api.Infrastructure.Repositories;
using FluentAssertions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Api.Tests.Unit.Infrastructure.Repositories;


public abstract class DbTestBase
{
    protected ISessionFactory SessionFactory = null!;
    protected ISession Session = null!;

    [SetUp]
    public void SetUpDb()
    {
        var cfg = new NHibernate.Cfg.Configuration();
        cfg.DataBaseIntegration(x =>
        {
            x.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
            x.Driver<NHibernate.Driver.SQLite20Driver>();
            x.Dialect<NHibernate.Dialect.SQLiteDialect>();
            x.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
        });
        FluentNHibernate.Cfg.Fluently.Configure(cfg)
            .Mappings(m => m.FluentMappings.Add<Api.Infrastructure.Mappings.EventMap>())
            .Mappings(m => m.FluentMappings.Add<Api.Infrastructure.Mappings.TicketSaleMap>())
            .BuildConfiguration();
        SessionFactory = cfg.BuildSessionFactory();
        Session = SessionFactory.OpenSession();
        new NHibernate.Tool.hbm2ddl.SchemaExport(cfg).Execute(false, true, false, Session.Connection, null);
    }

    [TearDown]
    public void TearDownDb()
    {
        Session.Dispose();
        SessionFactory.Dispose();
    }
}