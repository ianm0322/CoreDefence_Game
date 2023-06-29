using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShootingNode : ExecutionNode
{
    RobotAI _controller;

    public RobotShootingNode(RobotAI controller) : base()
    {
        this._controller = controller;
    }

    protected override BTState OnUpdate()
    {
        if (_controller.Target == null)
        {
            return BTState.Failure;
        }
        else
        {
            _controller.Shot();
            return BTState.Success;
        }
    }
}

public interface IShooting
{
    public void Shot();
}