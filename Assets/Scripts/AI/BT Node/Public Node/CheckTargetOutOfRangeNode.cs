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
        if (_target == null) return BTState.Failure;    // 타겟이 없으면 실패(계속)
        if (IsTargetTooFar()) return BTState.Success;   // 타겟이 너무 멀면 성공(탈출)
        if (CantDetect()) return BTState.Success;       // 타겟을 찾을 수 없으면 성공(탈출)
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
