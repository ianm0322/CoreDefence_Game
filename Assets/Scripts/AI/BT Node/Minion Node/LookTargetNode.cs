using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class LookTargetNode : EnemyAINode
{
    Transform _tr;

    public LookTargetNode(EnemyAI controller) : base(controller)
    {
        _tr = _controller.transform;
    }

    protected override BTState OnUpdate()
    {
        if (_controller.Target)
        {
            Vector3 look = (_controller.GetTarget().transform.position - _tr.position).normalized;
            look.y = 0;
            _tr.forward = Vector3.Lerp(_tr.forward, look, 5 * Time.deltaTime);
        }
        return BTState.Success;
    }
}
