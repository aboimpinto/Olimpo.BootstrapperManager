using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Olimpo;

public static class BootstrapperHostBuilder
{
    public static IHostBuilder RegisterEventAggregatorManager(this IHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, services) => 
        {
            services.AddSingleton<IBootstrapperManager, BootstrapperManager>();
        });

        return builder;
    }  
}
