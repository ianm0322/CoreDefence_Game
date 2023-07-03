using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class IsTargetExistNode : ExecutionNode
{
    ITargetter _controller;

    public IsTargetExistNode(ITargetter controller)
    {
        _controller = controller;
    }

    protected override BTState OnUpdate()
    {
        if (_controller.Target == null)
            return BTState.Failure;
        else
            return BTState.Success;
    }
}
