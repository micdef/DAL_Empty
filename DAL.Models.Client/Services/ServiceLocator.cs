using Microsoft.Extensions.DependencyInjection;
using Toolbox.Patterns;

namespace DAL.Models.Client.Services
{
    class ServiceLocator : ServLocator
    {
        private static ServiceLocator _instance;

        public static ServiceLocator Instance
        {
            get { return _instance ?? new ServiceLocator(); }
        }

        public ServiceLocator() { }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
        }
    }
}
