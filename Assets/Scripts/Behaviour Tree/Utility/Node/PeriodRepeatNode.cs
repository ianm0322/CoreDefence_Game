using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Unity
{
    /// <summary>
    /// (Decorator Node) ���� ��尡 ���� Ȥ�� Ż���� ������ ���� ��带 ���� �ֱ⸶�� �ݺ��Ѵ�.
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