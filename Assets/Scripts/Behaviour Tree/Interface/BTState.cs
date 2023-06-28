using System;

namespace BT
{
    [Flags]
    public enum BTState
    {
        Success             = 1,    
        Failure             = 2,
        Running             = 4,
        Abort               = 8,
    }
}