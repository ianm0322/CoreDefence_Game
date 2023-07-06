using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class DebugUtil
{ 
    [Conditional("DEBUG_LOG")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("DEBUG_LOG")]
    public static void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}
