using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class RepeatForSecondNode : DecoratorNode
    {
        private float _duration;
        private float _timer;

        private bool _stopOnSuccess;
        private bool _stopOnFail;

        public RepeatForSecondNode(float duration, BTNode content, bool stopOnSuccess = false, bool stopOnFail = false) : base(content)
        {
            _duration = duration;
            _stopOnFail = stopOnFail;
            _stopOnSuccess = stopOnSuccess;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _timer = Time.time;
        }

        protected override BTState OnUpdate()
        {
            if(Time.time - _timer < _duration)
            {
                var result = content.Evaluate();
                switch (result)
                {
                    case BTState.Success:
                        return _stopOnSuccess ? BTState.Success : BTState.Running;
                    case BTState.Failure:
                        return _stopOnFail ? BTState.Failure : BTState.Running;
                    case BTState.Running:
                        return BTState.Running;
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