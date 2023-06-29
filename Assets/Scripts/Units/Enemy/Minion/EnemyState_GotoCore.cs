using UnityEngine;
using UnityEngine.AI;

public class EnemyState_GotoCore : EnemyState
{
    private string state_readyToAttack = "MinionAttackReady";

    Vector3 corePos;
    public EnemyState_GotoCore(StateMachine self) : base("GotoCore", self)
    {

    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);
        corePos = GameManager.Instance.CorePosition;
        Self.Agent.SetDestination(corePos);

    }

    public override void OnStateExit(IState state)
    {
        base.OnStateExit(state);
        Self.Agent.SetDestination(Self.transform.position);
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
        DetectLogicUpdate();
    }

    private void DetectLogicUpdate()
    {
        if (CheckCoreIsNear())
        {
            Self.FocusTarget = (GameManager.Instance.Core.transform);
            Self.MoveState(state_readyToAttack);
            return;
        }
        Transform target = CheckNearObject();
        if (target != null) // 범위 내 공격대상 있으면 공격
        {
            Self.FocusTarget = (target.transform);
            Self.MoveState(state_readyToAttack);
        }
    }

    protected virtual bool CheckCoreIsNear()
    {
        return (corePos - Self.transform.position).sqrMagnitude < Self.data.DetectRange * Self.data.DetectRange;
    }

    protected virtual Transform CheckNearObject()
    {
        return Self.Scanner.ScanEntity();
    }
}
