using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class LookAtTargetNode : ExecutionNode
{
    private Transform _tr;
    private Vector3 _offsetAngle;
    private Vector3 _axisLock;
    private ITargetter _controller;

    public LookAtTargetNode(ITargetter controller, Transform tr, Vector3 offsetAngle, Vector3 axisLock)
    {
        _tr = tr;
        _controller = controller;
        _offsetAngle = offsetAngle;
        _axisLock = axisLock;
    }

    protected override BTState OnUpdate()
    {
        if (_controller.Target != null)
        {
            Vector3 v = _controller.Target.position - _tr.position;
            v.x *= _axisLock.x;
            v.y *= _axisLock.y;
            v.z *= _axisLock.z;
            _tr.forward = v;
            _tr.eulerAngles += _offsetAngle;
        }
        return BTState.Success;
    }
}
