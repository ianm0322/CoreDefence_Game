using System;
namespace BT
{
    /// <summary>
    /// (Decorator Node) 조건이 부합한 동안 내부 콘텐트 노드를 loop.
    /// </summary>
    public class WhileNode : DecoratorNode
    {
        private Func<bool> _condition;
        private bool _repeatOnSuccess;
        private bool _repeatOnFailure;

        public WhileNode(Func<bool> condition, BTNode content, bool repeatOnSuccess = false, bool repeatOnFailure = false) : base(content)
        {
            this._condition = condition;
            _repeatOnFailure = repeatOnFailure;
            _repeatOnSuccess = repeatOnSuccess;
        }

        protected override BTState OnUpdate()
        {
            if (_condition())
            {
                switch (content.Evaluate())
                {
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Success:
                        return _repeatOnSuccess ? BTState.Running : BTState.Success;
                    case BTState.Failure:
                        return _repeatOnFailure ? BTState.Running : BTState.Failure;
                    default:
                        return BTState.Failure;
                }
            }
            else
            {
                return BTState.Success;
            }
        }
    }
}