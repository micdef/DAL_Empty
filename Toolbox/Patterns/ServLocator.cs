using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbox.Patterns
{
    public abstract class ServLocator
    {
        protected IServiceProvider Container { get; set; }

        public ServLocator()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Container = serviceCollection.BuildServiceProvider();
        }

        public abstract void ConfigureServices(IServiceCollection serviceCollection);
    }
}
