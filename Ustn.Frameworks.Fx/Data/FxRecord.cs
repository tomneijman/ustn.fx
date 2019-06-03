using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ustn.Frameworks.Fx.Data
{
    public static class FxRecord
    {
        public static Dictionary<string, Dictionary<string, FxField>> Buffer = new Dictionary<string, Dictionary<string, FxField>>();

        public static void Add(string fileField, string value)
        {

        }

        public static void Add(string file, string field, string value)
        {
            //if (Buffer[file] != null) 

            //myDict.Add("Australia", "Canberra");

            //Buffer[file][field].ChangedValue = value;
        }
    }
}
