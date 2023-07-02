using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class TimerNode : ExecutionNode
    {
        private float _delay;
        private float _timer;

        private bool _isCheckOut = false;

        public TimerNode(float delay)
        {
            _delay = delay;
        }

        protected override BTState OnUpdate()
        {
            if (!_isCheckOut)
            {
                _timer = Time.time;
                _isCheckOut = true;
            }
            if(Time.time - _timer > _delay)
            {
                _isCheckOut = false;
                return BTState.Success;
            }
            else
            {
                return BTState.Failure;
            }
        }
    }
}