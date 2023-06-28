using BT;
using UnityEngine;

public abstract class MonoNode : BTNode
{
    protected GameObject gameObject;

    protected MonoNode(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}
