using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 조건 노드 실행 결과에 따라 
    /// </summary>
    public class IfElseNode : CompositeNode
    {
        BTNode _conditionNode, _successNode, _failureNode;
        BTState _result;

        bool _retCondResult = false;

        public IfElseNode(BTNode condition, BTNode successNode, BTNode failureNode) : base(condition, successNode, failureNode)
        {
            this._conditionNode = condition;
            this._successNode = successNode;
            this._failureNode = failureNode;
        }

        public IfElseNode(BTNode condition, BTNode successNode) : this(condition, successNode, null) { }

        public IfElseNode SetOption(bool returnConditionResult = false)
        {
            _retCondResult = returnConditionResult;
            return this;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _result = _conditionNode.Evaluate();
        }

        protected override BTState OnUpdate()
        {
            BTState subResult;
            switch (_result)
            {
                case BTState.Success:
                    if (_successNode == null) subResult = BTState.Success;
                    else subResult = _successNode.Evaluate();
                    return (_retCondResult) ? _result : subResult;
                case BTState.Failure:
                    if (_failureNode == null) subResult = BTState.Failure;
                    else subResult = _failureNode.Evaluate();
                    return (_retCondResult) ? _result : subResult;
                case BTState.Running:
                    return BTState.Running;
                default:
                    return BTState.Failure;
            }
        }
    }
}
