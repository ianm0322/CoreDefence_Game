using UnityEngine;
using UnityEngine.AI;

public class EnemyState_ChaseTarget : EnemyState
{
    private string state_gotoCore = "GotoCore";
    private string state_attack = "MinionAttack";

    float chaseTimer = 0f;
    float attackRange;

    Transform _selfTr;
    Transform _targetTr;

    public EnemyState_ChaseTarget(StateMachine self) : base("ChaseTarget", self)
    {
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);

        _targetTr = Self.FocusTarget.transform;
        _selfTr = Self.transform;

        attackRange = 0;
        Self.Agent.stoppingDistance = attackRange;
        chaseTimer = 0f;

    }

    public override void OnStateExit(IState state)
    {
        base.OnStateExit(state);
        Self.Agent.SetDestination(Self.transform.position);   // 에이전트 정지!
        Self.Agent.stoppingDistance = Self.data.AttackRange;
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        if (CanAttack()) Self.MoveState(state_attack); // 가까우면 공격
        if (!CanChase()) Self.MoveState(state_gotoCore); // 멀거나 이동 불가면 해제
        else
            Self.Agent.SetDestination(Self.FocusTarget.position);      // 타겟한테 이동
    }

    private bool CanAttack()
    {
        if (MathUtility.CompareDist(_targetTr.position - _selfTr.position, Self.data.AttackRange) < 0)   // 가까우면 공격 가능
        {
            return true;
        }
        return false;
    }

    private bool CanChase()
    {
        // 1. 거리가 충분히 먼가?
        if (IsDistTooFar())    // 너무 멀면 추적 불가
            return false;

        // 2. 갈 수 있는 곳인가?
        if (!IsAgentReachable())
        {
            return false;
        }
        return true;
    }

    private bool IsDistTooFar()
    {
        if (MathUtility.CompareDist(_targetTr.position - _selfTr.position, Self.data.AttackTargetRange) > 0)    // 너무 멀면 추적 불가
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= Self.data.TargetMissingDelay)
                return true;
        }
        else
        {
            chaseTimer = 0f;
        }
        return false;
    }

    private bool IsAgentReachable()
    {
        return Self.Scanner.CheckScanned(_targetTr);
    }
}
