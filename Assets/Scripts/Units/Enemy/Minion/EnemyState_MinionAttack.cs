using UnityEngine;

public class EnemyState_MinionAttack : EnemyState
{
    float attackTimer;
    bool isAttackDone;
    BaseState preState;

    public EnemyState_MinionAttack(StateMachine self) : base("MinionAttack", self)
    {
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);
        attackTimer = 0f;
        preState = state as BaseState;
        isAttackDone = false;
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
        attackTimer += Time.deltaTime * Self.data.AttackSpeed;

        if (isAttackDone == false && attackTimer > 9f / 30f)// && attackTimer < 0.12f)
        {
            if ((Self.FocusTarget.position - Self.transform.position).sqrMagnitude // 거리가 충분히 가까우면
                < Self.data.AttackRange * Self.data.AttackRange)
            {
                Attack();
                isAttackDone = true;
            }
        }
        if (attackTimer > 1)
        {
            Self.MoveState(preState);
        }
    }

    protected virtual void Attack()
    {
        Self.Attack();
    }
}
