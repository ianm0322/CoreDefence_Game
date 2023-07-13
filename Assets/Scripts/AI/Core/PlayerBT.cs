using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class PlayerBT : MonoBehaviour, IBehaviorTree
{
    RootNode _root;

    public void EndBT()
    {
        throw new System.NotImplementedException();
    }

    public RootNode MakeBT()
    {
        return new RootNode(
            new SelectorNode(
                new SequenceNode(
                    // Is Ground Node
                    )
                
                )
            );
    }

    public void Operate()
    {
        _root.Evaluate();
    }
}
