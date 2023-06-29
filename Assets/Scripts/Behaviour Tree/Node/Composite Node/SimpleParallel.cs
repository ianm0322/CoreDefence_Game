using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class SimpleParallel : CompositeNode
    {
        BTNode mainNode => childList[0];
        BTNode subNode => childList[1];

        bool _runSubOnSuccess = true;
        bool _runSubOnFail = false;
        BTState _result;

        public SimpleParallel(BTNode mainNode, BTNode subNode) : base(mainNode, subNode)
        {
            SetOption(true, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runSubOnSuccess">true: 메인 노드가 성공 시 서브 노드를 실행시킨다.</param>
        /// <param name="runSubOnFail">true: 메인 노드가 실패 시 서브 노드를 실행시킨다.</param>
        /// <returns></returns>
        public SimpleParallel SetOption(bool runSubOnSuccess = false, bool runSubOnFail = false)
        {
            _runSubOnSuccess = runSubOnSuccess;
            _runSubOnFail = runSubOnFail;
            return this;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _result = mainNode.Evaluate();
        }

        protected override BTState OnUpdate()
        {
            switch (_result)
            {
                case BTState.Success:
                    if(_runSubOnSuccess)
                    {
                        if(subNode.Evaluate()== BTState.Running)
                        {
                            return BTState.Running;
                        }
                    }
                    return BTState.Success;
                case BTState.Failure:
                    if (_runSubOnFail)
                    {
                        if (subNode.Evaluate() == BTState.Running)
                        {
                            return BTState.Running;
                        }
                    }
                    return BTState.Failure;
                case BTState.Running:
                    return BTState.Running;
                default:
                    throw new System.Exception();
            }
        }
    }
}
