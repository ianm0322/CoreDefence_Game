using BT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsFarNode : ExecutionNode
{
    bool _result;
    EnemyAI _me;
    Transform _target;
    float _dist;
    Func<Transform> getTarget;
    public TargetIsFarNode(EnemyAI me)
    {
        _me = me;
    }

    protected override BTState OnUpdate()
    {
        return BTState.Success;
    }
}
