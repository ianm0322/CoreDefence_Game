using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetNullNode : ExecutionNode
{
    ITargetter _controller;

    public SetTargetNullNode(ITargetter controller)
    {
        _controller = controller;
    }

    protected override BTState OnUpdate()
    {
        _controller.Target.GetComponent<CD_GameObject>().FocusCount--;
        _controller.Target = null;
        return BTState.Success;
    }
}
