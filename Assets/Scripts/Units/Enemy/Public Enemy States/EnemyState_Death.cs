using UnityEngine;

public class EnemyState_Death : EnemyState
{
    float attackTimer;

    public EnemyState_Death(StateMachine self) : base("Death", self)
    {
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);

        SetComponent(false);
        DestroyObject();
    }

    protected void SetComponent(bool enabled)
    {
        Self.Collider.isTrigger = enabled;
        Self.Agent.enabled = enabled;
        Self.Rigid.isKinematic = !enabled;
    }

    private void DestroyObject()
    {
        Object.Destroy(Self.gameObject, 5);
    }
}