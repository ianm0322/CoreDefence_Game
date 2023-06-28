using System;
namespace BT
{
    public class BranchNode : BTNode, IDecoratorNode
    {
        protected BTNode _successNode;
        protected BTNode _failureNode;
        protected Func<bool> _condition;
        protected bool _cond;

        public BranchNode(Func<bool> condition)
        {
            this._condition = condition;
        }

        public BranchNode(Func<bool> condition, BTNode content) : this(condition)
        {
            _successNode = content;
        }

        public BranchNode(Func<bool> condition, BTNode successNode, BTNode failureNode) : this(condition, successNode)
        {
            _failureNode = failureNode;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _cond = _condition();
        }

        protected override BTState OnUpdate()
        {
            BTState? result = null;
            if (_cond)
            {
                result = _successNode?.Evaluate();
            }
            else
            {
                result = _failureNode?.Evaluate();
            }

            switch (result)
            {
                case BTState.Running:
                    return BTState.Running;
                case BTState.Success:
                    return BTState.Success;
                case BTState.Failure:
                    return BTState.Failure;
                case BTState.Abort:
                    return BTState.Abort;
                case null:
                    return _cond ? BTState.Success : BTState.Failure;
            }

            throw new Exception();
        }
    }
}