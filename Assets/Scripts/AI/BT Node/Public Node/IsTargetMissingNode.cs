using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetMissingNode : EnemyAINode
{
    private Transform _me;
    private Transform _target;
    private EnemyData _data;

    public IsTargetMissingNode(EnemyAI controller) : base(controller)
    {
        _me = controller.transform;
        _data = controller.Data;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        _target = _controller.Target;
    }

    protected override BTState OnUpdate()
    {
        if (IsTargetTooFar()) return BTState.Failure;
        if (CantDetect()) return BTState.Failure;
        else return BTState.Success;
    }

    private bool IsTargetTooFar()
    {
        if (MathUtility.CompareDist(_me.position - _target.position, _data.TargetMissingRange) > 0)
        {
            return false;
        }
        return true;
    }

    private bool CantDetect()
    {
        return _controller.Scanner.CheckScanned(_target) == false;
    }
}
