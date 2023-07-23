using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class LookAtTargetNode : ExecutionNode
{
    private Transform _tr;
    private Vector3 _offsetAngle;
    private Vector3 _offsetMultiple;
    private AxisOrder _axisOrder;
    private ITargetter _controller;

    public LookAtTargetNode(ITargetter controller, Transform tr, Vector3 offsetAngle, Vector3 offsetMultiple, AxisOrder axisOrder)
    {
        _tr = tr;
        _controller = controller;
        _offsetAngle = offsetAngle;
        _offsetMultiple = offsetMultiple;
        _axisOrder = axisOrder;
    }

    protected override BTState OnUpdate()
    {
        if (_controller.GetTarget() != null)
        {
            Vector3 angle;

            angle = Quaternion.LookRotation(_controller.GetTarget().transform.position - _tr.position).eulerAngles;
            angle = _axisOrder.Transform(angle);
            angle.x *= _offsetMultiple.x;
            angle.y *= _offsetMultiple.y;
            angle.z *= _offsetMultiple.z;
            angle += _offsetAngle;
            _tr.eulerAngles = angle;
        }
        return BTState.Success;
    }
}

public struct AxisOrder
{
    public static AxisOrder XYZ = new AxisOrder(AxisType.X, AxisType.Y, AxisType.Z);
    public static AxisOrder XZY = new AxisOrder(AxisType.X, AxisType.Z, AxisType.Y);
    public static AxisOrder YXZ = new AxisOrder(AxisType.Y, AxisType.X, AxisType.Z);
    public static AxisOrder YZX = new AxisOrder(AxisType.Y, AxisType.Z, AxisType.X);
    public static AxisOrder ZXY = new AxisOrder(AxisType.Z, AxisType.X, AxisType.Y);
    public static AxisOrder ZYX = new AxisOrder(AxisType.Z, AxisType.Y, AxisType.X);

    public AxisType x;
    public AxisType y;
    public AxisType z;

    public AxisOrder(AxisType x, AxisType y, AxisType z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 Transform(Vector3 v)
    {
        Vector3 result = Vector3.zero;
        result.x = (x == AxisType.X) ? v.x : ((x == AxisType.Y) ? v.y : ((x == AxisType.Z) ? v.z : 0));
        result.y = (y == AxisType.X) ? v.x : ((y == AxisType.Y) ? v.y : ((y == AxisType.Z) ? v.z : 0));
        result.z = (z == AxisType.X) ? v.x : ((z == AxisType.Y) ? v.y : ((z == AxisType.Z) ? v.z : 0));
        return result;
    }
}

public enum AxisType
{
    X, Y, Z, None
}