using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Toolbox.ADO;
using Toolbox.Patterns;

namespace DAL.Models.Global.Services
{
    public class ServiceLocator : ServLocator
    {
        private static ServiceLocator _instance;

        public static ServiceLocator Instance
        {
            get { return _instance ?? new ServiceLocator(); }
        }

        private ServiceLocator() { }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DbProviderFactory, SqlClientFactory>((sp) => SqlClientFactory.Instance);
            serviceCollection.AddSingleton<IConnectionInfo, ConnectionInfo>((sp) => new ConnectionInfo(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString));
            serviceCollection.AddSingleton<IConnection, Connection>();
        }

        internal IConnection Connection
        {
            get { return Container.GetService<IConnection>(); }
        }
    }
}
