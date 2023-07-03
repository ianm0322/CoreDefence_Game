using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class IsParalysisNode : ExecutionNode
{
    AIController _controller;

    public IsParalysisNode(AIController controller)
    {
        _controller = controller;
    }

    protected override BTState OnUpdate()
    {
        if (_controller.IsParalysis == true)
            return BTState.Success;
        else
            return BTState.Failure;
    }
}
