using Microsoft.Extensions.Logging;

namespace Olimpo;

public class BootstrapperManager : IBootstrapperManager
{
    private readonly IEnumerable<IBootstrapper> _bootstrappers;
    private readonly ILogger<BootstrapperManager> _logger;

    public BootstrapperManager(
        IEnumerable<IBootstrapper> bootstrappers,
        ILogger<BootstrapperManager> logger)
    {
        this._bootstrappers = bootstrappers;
        this._logger = logger;
    }

    public async Task Start()
    {
        var orderedBootstrappers = this._bootstrappers.OrderBy(x => x.Priority);
        foreach(var module in orderedBootstrappers)
        {
            this._logger.LogInformation($"Starting {module.GetType().Name}...");
            await module.Startup();
        }    
    }
}
