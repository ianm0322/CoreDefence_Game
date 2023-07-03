using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    public class ConditionalAbortNode : DecoratorNode
    {
        private Func<bool> _condition;
        private bool _abortSelf;
        private bool _abortLowerPeriority;

        private bool _result;

        public ConditionalAbortNode(Func<bool> condition, bool abortSelf, bool abortLowerPeriority, BTNode content) : base(content)
        {
            this._condition = condition;
            this._abortSelf = abortSelf;
            this._abortLowerPeriority = abortLowerPeriority;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _result = _condition();
        }

        protected override BTState OnUpdate()
        {
            throw new NotImplementedException();
            //if (_result)
            //{
            //    if (_abortLowerPeriority)
            //    {

            //    }
            //    else
            //    {
            //        var result = content.Evaluate();
            //        switch (result)
            //        {
            //            case BTState.Success:
            //                break;
            //            case BTState.Failure:
            //                break;
            //            case BTState.Running:
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //}
        }
    }
}
