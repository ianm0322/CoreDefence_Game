using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class IsDiedNode : ExecutionNode
{
    private CD_GameObject _entity;

    public IsDiedNode(CD_GameObject entity)
    {
        this._entity = entity;
    }

    protected override BTState OnUpdate()
    {
        if (_entity.IsDied)
        {
            return BTState.Success;
        }
        else
        {
            return BTState.Failure;
        }
    }
}
