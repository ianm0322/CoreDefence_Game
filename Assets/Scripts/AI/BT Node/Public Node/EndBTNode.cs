using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBTNode : ExecutionNode
{
    IBehaviorTree _bt;

    public EndBTNode(IBehaviorTree bt)
    {
        this._bt = bt;
    }

    protected override BTState OnUpdate()
    {
        _bt.EndBT();
        return BTState.Running;
    }
}
