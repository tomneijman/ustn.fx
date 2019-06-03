using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ustn.Frameworks.Fx.System;


namespace Ustn.Frameworks.Fx.Data
{
    public static class FxDatabaseSettings
    {
        public static string GetConnectionString()
        {
            var config = FX.Instance.ServiceProvider.GetService<IConfiguration>();
            return config.GetConnectionString("DefaultConnection");
        }

        public static string GetProviderName()
        {
            var config = FX.Instance.ServiceProvider.GetService<IConfiguration>();
            return config.GetSection("DbProviderFactory").GetSection("name").Value;
        }

        public static string GetProviderInvariant()
        {
            var config = FX.Instance.ServiceProvider.GetService<IConfiguration>();
            return config.GetSection("DbProviderFactory").GetSection("invariant").Value;
        }

        public static string GetProviderType()
        {
            var config = FX.Instance.ServiceProvider.GetService<IConfiguration>();
            return config.GetSection("DbProviderFactory").GetSection("type").Value;
        }
    }
}
