using System.Reactive.Subjects;

namespace Olimpo;

public interface IBootstrapper
{
    Subject<bool> BootstrapFinished { get; }

    int Priority { get; set; }

    Task Startup();

    void Shutdown();
}
