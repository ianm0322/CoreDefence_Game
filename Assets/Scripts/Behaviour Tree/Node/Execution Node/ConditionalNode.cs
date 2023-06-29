using System;

namespace BT
{
    public class ConditionalNode : ExecutionNode
    {
        protected Func<bool> _condition;
        protected bool _result;

        public ConditionalNode(Func<bool> condition)
        {
            _condition += condition;
        }

        public ConditionalNode(bool boolean)
        {
            _condition = null;
            _result = boolean;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            if(_condition != null)
                _result = _condition();
        }

        protected override BTState OnUpdate()
        {
            if (_result)
            {
                return BTState.Success;
            }
            else
            {
                return BTState.Failure;
            }
        }
    }
}