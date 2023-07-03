using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class WaitForAttackSpeedNode : ExecutionNode
{
    EntityData _data;
    float _timer;

    public WaitForAttackSpeedNode(EntityData data)
    {
        _data = data;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        _timer = 0;
    }

    protected override BTState OnUpdate()
    {
        _timer += Time.deltaTime * _data.AttackSpeed;
        if (_timer >= 1f)
        {
            return BTState.Success;
        }
        else
        {
            return BTState.Running;
        }
    }
}
