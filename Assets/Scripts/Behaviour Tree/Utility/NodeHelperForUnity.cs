using UnityEngine;

namespace BT.Unity
{
    public static class NodeHelperForUnity
    {
        public static BTNode Log(string log, BTState result = BTState.Success) => new DebugNodes.DebugNode_Log(log, result);
        public static BTNode Log(string log, BTNode content) => new DebugNodes.DebugNode_DecoLog(log, content);
        public static BTNode LogResult(BTNode content) => new DebugNodes.DebugNode_LogResult(content);
    }
}