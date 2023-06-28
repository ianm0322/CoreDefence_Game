using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 초근접에서만 공격. 
/// </summary>
public class EnemyState_MinionAttackReady : EnemyState
{
    private string state_default = "GotoCore";
    private string state_chasing = "ChaseTarget";
    private string state_attack = "MinionAttack";

    float attackTimer;  // 공격 한 후부터 카운팅되는 타이머.

    public EnemyState_MinionAttackReady(StateMachine self) : base("MinionAttackReady", self)
    {
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);

        Self.Agent.SetDestination(Self.transform.position); // 제자리 멈춤.
        string type = (state as BaseState)?.StateType;
        if (type == state_default || type == state_chasing)  // 만약 최초 타겟팅이면 즉시 공격
            attackTimer = Self.data.AttackDelay;
        else
            attackTimer = 0f;
    }

    public override void OnStateExit(IState state)
    {
        base.OnStateExit(state);
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        attackTimer += Time.deltaTime;

        Vector3 look = (Self.FocusTarget.position - Self.transform.position).normalized;
        look.y = 0;
        Self.transform.forward = Vector3.Lerp(Self.transform.forward, look, 5 * Time.deltaTime);

        // 타겟과 거리가 충분한지 검사한다. 거리가 너무 멀면 타겟팅이 해제되고, 거리가 가깝고 공격 타이밍이 맞으면 공격한다.
        float targetDist = (Self.FocusTarget.position - Self.transform.position).magnitude;
        if (targetDist <= Self.data.AttackRange) // 공격범위 내면 공격 가능한지 검사.
        {
            if (attackTimer >= Self.data.AttackDelay)   // 공격 타이밍이 맞으면 공격.
            {
                Self.MoveState(state_attack);
            }
        }
        else// if (targetDist > Self.data.AttackRange) // 공격범위 밖이면 적을 추적함.
        {
            Self.MoveState(state_chasing);
        }
    }

    protected virtual void Attack()
    {
        Self.Attack();
    }
}
