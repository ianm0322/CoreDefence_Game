using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// ���� ��尡 ���� �Ⱓ ��� �����ؾ� Success�� ��ȯ�ϴ� �޼���.
    /// </summary>
    public class TimeOverNode : DecoratorNode
    {
        float _successTime;
        float _timer = 0;

        bool _successOnce = true;
        bool _abortOnTimeOver = false;

        public TimeOverNode(float successTime, BTNode content) : base(content)
        {
            _successTime = successTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="successOnce">�� �ɼ��� true�� success�� ��ȯ�� �� Ÿ�̸Ӹ� �ʱ�ȭ�Ѵ�.</param>
        /// <param name="abortOnTimeOver">�� �ɼ��� true�� ���� ��尡 running���̶� �ð��� �ʰ��Ǹ� running�� �ߴ��ϰ� ����� ��ȯ�Ѵ�.</param>
        /// <returns></returns>
        public TimeOverNode SetOption(bool successOnce = true, bool abortOnTimeOver = false)
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
                case BTState.Failure:   // ���н� Ÿ�� �ʱ�ȭ
                    _timer = 0;
                    return BTState.Failure;
                case BTState.Running:
                    if (_abortOnTimeOver)  // �ð� �ʰ��� Ż�� �ɼ��� on�̶�� Ÿ�̸� ���� �� running�� ������ �ٷ� success ��ȯ
                    {
                        if (IsTimeOver()) return BTState.Success;
                        else return BTState.Running;
                    }
                    else return BTState.Running;
                default:
                    return BTState.Failure;
            }
        }
        
        // �ð� �������� false ��ȯ, �ƴϸ� true ��ȯ
        private bool IsTimeOver()
        {
            if (_timer > _successTime)
            {
                if (_successOnce)   // 1���� ���� ��ȯ�̸� Ÿ�̸� �ʱ�ȭ. �ƴϸ� ��� success ��ȯ
                {
                    _timer = 0f;
                }
                return true;
            }
            return false;
        }
    }
}
