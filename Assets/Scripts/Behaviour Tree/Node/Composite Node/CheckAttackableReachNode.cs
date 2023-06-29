using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class CheckAttackableReachNode : EnemyAINode
{
    public CheckAttackableReachNode(EnemyAI controller) : base(controller)
    {
    }

    protected override BTState OnUpdate()
    {
        if(IsNear())
        {
            return BTState.Success;
        }
        return BTState.Failure;

    }

    private bool IsNear()
    {
        return MathUtility.CompareDist(_controller.Target.position - _controller.transform.position, _controller.Data.AttackTargetRange) < 0;
    }
}
