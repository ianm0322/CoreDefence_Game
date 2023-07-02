using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShootingNode : ExecutionNode
{
    RobotAI _controller;
    float _timer;
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
            _timer += Time.deltaTime * _controller.Data.AttackSpeed;
            if (_timer > 1f)
            {
                _timer = 0;
                _controller.Shot();
                return BTState.Success;
            }
        }
        return BTState.Success;
    }
}

public interface IShooter
{
    public void Shot();
}