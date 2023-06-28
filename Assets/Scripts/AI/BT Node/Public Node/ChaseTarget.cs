using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : EnemyAINode
{
    public ChaseTarget(EnemyAI controller) : base(controller)
    {
    }

    protected override BTState OnUpdate()
    {
        _controller.Agent.SetDestination(_controller.Target.position);
        return BTState.Success;
    }
}
