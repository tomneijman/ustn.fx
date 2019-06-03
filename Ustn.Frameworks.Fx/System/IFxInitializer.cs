using Microsoft.Extensions.DependencyInjection;

namespace Ustn.Frameworks.Fx.System
{
    public interface IFxInitializer
    {
        IServiceCollection Initialize(IServiceCollection serviceCollection);
    }
}
