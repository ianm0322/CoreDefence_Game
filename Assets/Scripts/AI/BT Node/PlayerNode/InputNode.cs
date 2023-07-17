using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class InputNode : ExecutionNode
{
    KeyCode _key;

    public InputNode(KeyCode key)
    {
        this._key = key;
    }

    protected override BTState OnUpdate()
    {
        if (Input.GetKey(_key))
        {
            return BTState.Success;
        }
        else
        {
            return BTState.Failure;
        }
    }
}
