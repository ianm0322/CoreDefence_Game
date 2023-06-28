using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAINode : BTNode
{
    protected EnemyAI _controller;

    public EnemyAINode(EnemyAI controller)
    {
        _controller = controller;
    }
}
