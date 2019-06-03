using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ustn.Frameworks.Fx.System
{
    // Singleton!
    // For initializing the Framework
    // Fx class is tightly coupled with IServiceCollection
    public sealed class FX
    {
        private static readonly Lazy<FX> Lazy = new Lazy<FX>(() => new FX());

        private bool _isInitialized;
        public IServiceCollection ServiceCollection { get; private set; }
        public IServiceProvider ServiceProvider { get; set; }

        private FX()
        {
            Initialize();
        }

        public static FX Instance => Lazy.Value;

        public IServiceCollection Initialize<T>(T obj) where T : IFxInitializer
        {
            return obj.Initialize(ServiceCollection);
        }

        private void Initialize()
        {
            if (!_isInitialized)
            {
                ServiceCollection = new ServiceCollection();
                _isInitialized = true;

                // Todo populate ServiceCollection
            }
        }
        public void BuildServiceProvider()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
    }
}
