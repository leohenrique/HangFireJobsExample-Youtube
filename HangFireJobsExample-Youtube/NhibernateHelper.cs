using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HangFireJobsExample_Youtube.Domain;
using NHibernate;
using System.Data.SqlClient;

namespace HangFireJobsExample_Youtube
{
    public class NHibernateHelper
    {
        public static ISessionFactory CreateSessionFactory()
        {

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(@"Server=localhost;Database=example_multi;Trusted_Connection=True;Encrypt=False;"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsuarioMap>())                
                .BuildSessionFactory();
        }
    }
}
