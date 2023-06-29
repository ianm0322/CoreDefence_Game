using UnityEngine;

namespace BT.DebugNodes
{
    public class DebugNode_LogResult : DebugDecoratorNode
    {
        public DebugNode_LogResult(BTNode content) : base(content)
        {
        }

        protected override BTState OnUpdate()
        {
            var result = content.Evaluate();
            return result;
        }
    }
}
