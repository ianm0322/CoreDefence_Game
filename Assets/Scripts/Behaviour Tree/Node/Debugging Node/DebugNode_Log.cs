using UnityEngine;

namespace BT.DebugNodes
{
    public class DebugNode_Log : DebugExecutionNode
    {
        private string _log;
        private BTState _result;

        public DebugNode_Log(string log, BTState result)
        {
            this._log = log;
            this._result = result;
        }
        protected override BTState OnUpdate()
        {
            MyDebug.Log(_log);
            return _result;
        }
    }
}