using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour, IBehaviorTree
{
    public int instanceId;

    public RootNode Root { get; protected set; }
    public BTBlackboard Blackboard { get; protected set; }

    protected virtual void Awake()
    {
        Blackboard = new BTBlackboard(this);
    }

    public void ResetBT()
    {
        Blackboard.DestroyBlackboard();
        Root = MakeBT();
    }

    public abstract RootNode MakeBT();

    public void Operate()
    {
        if (Root == null)
            Root = MakeBT();
        Root.Evaluate();
    }

    public void EndBT()
    {
        //Root = null;
    }
}
