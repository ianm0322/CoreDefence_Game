using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetNode : EnemyAINode
{
    public SetTargetNode(EnemyAI controller) : base(controller)
    {
    }

    protected override BTState OnUpdate()
    {
        _controller.Target = _controller.Scanner.ScanEntity();
        if(_controller.Target == null)
        {
            return BTState.Failure;
        }
        else
        {
            return BTState.Success;
        }
    }
}
