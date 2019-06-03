using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ustn.Frameworks.Fx.Data
{
    public class FxField
    {
        public string File { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public object ChangedValue { get; set; }
        public FxRecordBuffer Options { get; set; }
    }
}
