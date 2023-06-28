using System;
namespace BT
{
    /// <summary>
    /// 
    /// </summary>
    public class RepeatNode : DecoratorNode
    {
        private int _end;
        private int _current = 0;
        private bool _repeatOnSuccess;
        private bool _repeatOnFailure;

        public RepeatNode(int count, BTNode content, bool repeatOnSuccess = false, bool repeatOnFailure = false) : base(content)
        {
            this._end = count;
            _repeatOnFailure = repeatOnFailure;
            _repeatOnSuccess = repeatOnSuccess;
        }

        protected override BTState OnUpdate()
        {
            if (State != BTState.Running)
                _current = 0;

            if (_current < _end)
            {
                _current++;
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
                _current = 0;
                return BTState.Success;
            }
        }
    }
}