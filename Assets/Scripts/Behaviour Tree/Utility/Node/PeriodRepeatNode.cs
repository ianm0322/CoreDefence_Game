using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Unity
{
    /// <summary>
    /// (Decorator Node) 하위 노드가 실패 혹은 탈출할 때까지 하위 노드를 일정 주기마다 반복한다.
    /// </summary>
    public class PeriodRepeatNode : DecoratorNode
    {
        private float _cycleTime;
        private float _timer;

        public PeriodRepeatNode(float cycle, BTNode content) : base(content)
        {
            this._cycleTime = cycle;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _timer = 0f;
        }

        protected override BTState OnUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer >= _cycleTime)
            {
                _timer -= _cycleTime;
                BTState result = content.Evaluate();
                switch (result)
                {
                    case BTState.Success:
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Failure:
                        return BTState.Success;
                    case BTState.Abort:
                        return BTState.Failure;
                    default:
                        throw new System.Exception();
                }
            }
            else
            {
                return BTState.Running;
            }
        }
    }
}