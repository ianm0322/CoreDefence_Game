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

        public RepeatNode(int count, BTNode content) : base(content)
        {
            this._end = count;
            SetOption();
        }

        public RepeatNode SetOption(bool repeatOnSuccess = false, bool repeatOnFailure = false)
        {
            _repeatOnFailure = repeatOnFailure;
            _repeatOnSuccess = repeatOnSuccess;
            return this;
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