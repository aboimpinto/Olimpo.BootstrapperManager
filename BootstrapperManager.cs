using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;

namespace Olimpo;

public class BootstrapperManager : IBootstrapperManager
{
    private readonly IEnumerable<IBootstrapper> _bootstrappers;
    private readonly ILogger<BootstrapperManager> _logger;

    public int ModulesCount { get; }

    public Subject<bool> AllModulesBootstrapped { get; }

    public Subject<bool> ModuleBootstrapped { get; }

    public BootstrapperManager(
        IEnumerable<IBootstrapper> bootstrappers,
        ILogger<BootstrapperManager> logger)
    {
        this._bootstrappers = bootstrappers;
        this._logger = logger;

        this.AllModulesBootstrapped = new Subject<bool>();
        this.ModuleBootstrapped = new Subject<bool>();
        this.ModulesCount = this._bootstrappers.Count();
    }

    public async Task Start()
    {
        var moduleCount = this._bootstrappers.Count();
        var moduleBootstrapped = 0 ;

        var orderedBootstrappers = this._bootstrappers.OrderBy(x => x.Priority);
        foreach(var module in orderedBootstrappers)
        {
            module.BootstrapFinished.Subscribe(x => 
            {
                if (x)
                {
                    moduleBootstrapped ++;
                    this.ModuleBootstrapped.OnNext(true);
                }

                if (moduleBootstrapped >= moduleCount)
                {
                    this.AllModulesBootstrapped.OnNext(true);
                }
            });

            this._logger.LogInformation($"Starting {module.GetType().Name}...");
            await module.Startup();
        }    
    }
}
