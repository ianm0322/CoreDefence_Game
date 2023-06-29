using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetNode : EnemyAINode
{
    Transform _transform = null;

    public ChaseTargetNode(EnemyAI controller) : base(controller)
    {
        _transform = null;
    }

    public ChaseTargetNode(EnemyAI controller, Transform transform) : this(controller)
    {
        _transform = transform;
    }

    protected override BTState OnUpdate()
    {
        if (_transform)
        {
            _controller.Agent.SetDestination(_transform.position);
        }
        else
        {
            _controller.Agent.SetDestination(_controller.Target.position);
        }
        return BTState.Success;
    }
}
