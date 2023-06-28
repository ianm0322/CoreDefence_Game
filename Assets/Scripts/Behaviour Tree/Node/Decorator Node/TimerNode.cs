using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 하위 노드가 일정 기간 계속 성공해야 Success를 반환하는 메서드.
    /// </summary>
    public class TimerNode : DecoratorNode
    {
        float _successTime;
        float _timer = 0;

        bool _successOnce;
        bool _abortOnTimeOver;

        public TimerNode(float successTime, BTNode content) : base(content)
        {
            _successTime = successTime;
        }

        public TimerNode SetOption(bool successOnce = true, bool abortOnTimeOver = false)
        {
            _successOnce = successOnce;
            _abortOnTimeOver = abortOnTimeOver;
            return this;
        }

        protected override BTState OnUpdate()
        {
            var result = content.Evaluate();
            _timer += Time.deltaTime;
            switch (result)
            {
                case BTState.Success:
                    if (IsTimeOver()) return BTState.Success;
                    else return BTState.Failure;
                case BTState.Failure:   // 실패시 타임 초기화
                    _timer = 0;
                    return BTState.Failure;
                case BTState.Running:
                    if (_abortOnTimeOver)  // 시간 초과시 탈출 옵션이 on이라면 타이머 종료 시 running을 끝내고 바로 success 반환
                    {
                        if (IsTimeOver()) return BTState.Success;
                        else return BTState.Running;
                    }
                    else return BTState.Running;
                default:
                    return BTState.Failure;
            }
        }
        
        // 시간 지났으면 false 반환, 아니면 true 반환
        private bool IsTimeOver()
        {
            if (_timer > _successTime)
            {
                if (_successOnce)   // 1번만 성공 반환이면 타이머 초기화. 아니면 계속 success 반환
                {
                    _timer = 0f;
                }
                return true;
            }
            return false;
        }
    }
}
