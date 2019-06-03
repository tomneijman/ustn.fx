using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ustn.Frameworks.Fx.Data;
using Ustn.Frameworks.Fx.System;

namespace FxFrameworkTester
{
    class Program
    {
        private static IConfiguration _config;
        private static IServiceCollection _services;

        static void Main(string[] args)
        {
            Startup();

            var ddSitemap = new FxDataDictionary("bgc_sitemap");
            var ddSchema = new FxDataDictionary("bgc_schema");
            var ddComponent = new FxDataDictionary("bgc_component");
            ddComponent.AttachServer(ddSchema);
            ddComponent.AttachServer(ddSitemap);


            ddComponent.SetDefaultValue("name", "Papa Tom");
            ddComponent.ClearRecordBuffer();

            Console.WriteLine($"Default {ddComponent.GetChangedValue("name")}");
         
            ddComponent.FindById(999);

            Console.WriteLine($"COMPONENT: {ddComponent.GetValue("name")}");
            Console.WriteLine($"SCHEMA: {ddSchema.CurrentRecord["name"].Value}");
            Console.WriteLine($"SITEMAP: {ddSitemap.CurrentRecord["name"].Value}");

            ddSitemap.CurrentRecord["name"].Value = "JOOP";
            Console.WriteLine($"SITEMAP NEW: {ddSitemap.GetValue("name")}");
  
            ddComponent.CurrentRecord["name"].ChangedValue = ddComponent.GetValue("name") + " - JAS";

            ddComponent.SetChangedValue("name", "Jose is liever");
            ddComponent.RequestSave();

            Console.WriteLine("EINDE TEST");
            Console.ReadLine();
        }

        private static void Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true);

            _config = builder.Build();

            ConfigureServices();
        }

        private static void ConfigureServices()
        {
            _services = FX.Instance.ServiceCollection;
            _services.AddSingleton(_config);
            FX.Instance.BuildServiceProvider();         
        }
    }
}