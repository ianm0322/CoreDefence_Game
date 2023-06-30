using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetNullNode : EnemyAINode
{
    public SetTargetNullNode(EnemyAI controller) : base(controller)
    {
    }

    protected override BTState OnUpdate()
    {
        _controller.Target = null;
        return BTState.Success;
    }
}
