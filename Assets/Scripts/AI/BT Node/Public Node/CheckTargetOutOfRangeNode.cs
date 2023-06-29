using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetOutOfRangeNode : EnemyAINode
{
    private Transform _me;
    private Transform _target;
    private EnemyData _data;

    public CheckTargetOutOfRangeNode(EnemyAI controller) : base(controller)
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
        if (IsTargetTooFar()) return BTState.Success;
        if (CantDetect()) return BTState.Success;
        else return BTState.Failure;
    }

    private bool IsTargetTooFar()
    {
        if (MathUtility.CompareDist(_me.position - _target.position, _data.TargetMissingRange) > 0)
        {
            return true;
        }
        return false;
    }

    private bool CantDetect()
    {
        return _controller.Scanner.CheckScanned(_target) == false;
    }
}
