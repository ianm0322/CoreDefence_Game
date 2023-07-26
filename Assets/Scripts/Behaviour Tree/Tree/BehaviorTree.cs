using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour, IBehaviorTree
{
    public RootNode Root { get; protected set; }

    public void StartBT()
    {
        Root = MakeBT();
    }

    public abstract RootNode MakeBT();

    public void Operate()
    {
        if (Root == null)
            Root = MakeBT();
        Root.Evaluate();
    }

    protected virtual void OnDestroy()
    {
        Root = null;
    }
}
