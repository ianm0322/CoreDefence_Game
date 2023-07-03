using UnityEngine;
using BT;

public class WaitForAttackDelayNode : ExecutionNode
{
    EntityData _data;
    float _timer;

    public WaitForAttackDelayNode(EntityData data)
    {
        _data = data;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        _timer = 0;
    }

    protected override BTState OnUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= _data.AttackDelay)
        {
            return BTState.Success;
        }
        else
        {
            return BTState.Running;
        }
    }
}
