using System;

namespace Ustn.Frameworks.Fx.System.Enums
{
    // First version not nescescarry
    [Flags]
    public enum DelegationMode
    {
        None = 0,
        AutoDelegate = 1,
        DelegateToParent = 2,
        NoDelegate = 4
    }
}
