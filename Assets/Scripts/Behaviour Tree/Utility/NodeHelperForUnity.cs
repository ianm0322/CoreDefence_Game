using UnityEngine;

namespace BT.Unity
{
    public static class NodeHelperForUnity
    {
        public static DebugNodes.DebugNode_Log LeafLog(string log, BTState result = BTState.Success) => new DebugNodes.DebugNode_Log(log, result);
        public static DebugNodes.DebugNode_DecoLog Log(string log, BTNode content) => new DebugNodes.DebugNode_DecoLog(log, content);
        public static DebugNodes.DebugNode_LogResult LogResult(BTNode content) => new DebugNodes.DebugNode_LogResult(content);
    }
}