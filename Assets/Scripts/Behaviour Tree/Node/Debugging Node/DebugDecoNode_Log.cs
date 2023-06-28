using UnityEngine;

namespace BT.DebugNodes
{
    public class DebugNode_DecoLog : DebugDecoratorNode
    {
        private string _log;

        public DebugNode_DecoLog(string log, BTNode content) : base(content)
        {
            this._log = log;
        }

        protected override BTState OnUpdate()
        {
            if (State != BTState.Running)
                Debug.Log(_log);
            return content.Evaluate();
        }
    }
}
