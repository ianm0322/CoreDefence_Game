using System;

namespace BT
{
    [Flags]
    public enum RepeatNodeEnum
    {
        None                = 0,
        RepeatOnSuccess     = 1,
        RepeatOnFailure     = 2,
        Both                = RepeatOnSuccess | RepeatOnFailure
    }
}