using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetOutOfRangeNode : ExecutionNode
{
    private Transform _selfTr;
    private ITargetter _targetter;
    private AIData _data;

    public CheckTargetOutOfRangeNode(Transform selfTr, ITargetter targetter, AIData data) : base()
    {
        _selfTr = selfTr;
        _targetter = targetter;
        _data = data;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
    }

    protected override BTState OnUpdate()
    {
        if (_targetter.GetTarget() == null) 
            return BTState.Failure;    // Ÿ���� ������ ����(���)

        if (IsTargetTooFar()) 
            return BTState.Success;   // Ÿ���� �ʹ� �ָ� ����(Ż��)

        if (CantDetect()) 
            return BTState.Success;       // Ÿ���� ã�� �� ������ ����(Ż��)

        else 
            return BTState.Failure;
    }

    private bool IsTargetTooFar()
    {
        if (MathUtility.CompareDist(_selfTr.position - _targetter.GetTarget().transform.position, _data.TargetMissingRange) > 0)
        {
            return true;
        }
        return false;
    }

    private bool CantDetect()
    {
        return _targetter.GetTargetSelector().Evaluate(_targetter.GetTarget());
    }
}
