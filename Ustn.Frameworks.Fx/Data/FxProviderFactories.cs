using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ustn.Frameworks.Fx.Data
{
    public static class FxProviderFactories
    {
        public static void RegisterFactories()
        {
            DbProviderFactories.RegisterFactory(FxDatabaseSettings.GetProviderInvariant(), FxDatabaseSettings.GetProviderType());
        }
    }
}
