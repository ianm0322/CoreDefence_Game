using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class TryNode : DecoratorNode
    {
        private Func<bool> _condition;
        private bool _result;

        public TryNode(Func<bool> condition, BTNode content) : base(content)
        {
            _condition = condition;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _result = _condition();
        }

        protected override BTState OnUpdate()
        {
            if (_result)
            {
                BTState state = content.Evaluate();
                return state;
            }
            else
            {
                return BTState.Success;
            }
        }
    }
}
