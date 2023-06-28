using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour, IBehaviorTree
{
    INode _root;

    public abstract RootNode GenerateBT();

    public void Operate()
    {
        _root.Evaluate();
    }
}
