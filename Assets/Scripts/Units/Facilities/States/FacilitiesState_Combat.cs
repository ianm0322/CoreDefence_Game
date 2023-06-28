using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilitiesState_Combat : FacilitiesState_Base
{
    float attackTimer;

    public FacilitiesState_Combat(StateMachine machine) : base("Combat", machine)
    {
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        if (Self.IsTargetPosible() == false)    // 타겟이 범위 밖으로 나가면 재탐색
        {
            if (Self.SearchTarget() == false)   // 재탐색했어도 없으면 대기 상태로 변경
            {
                //Self.MoveState("Idle");
            }
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();

        attackTimer += Time.fixedDeltaTime * Self.AttackSpeed;
        if(attackTimer > Self.AttackDelay)
        {
            if (Self.SearchTarget())
            {
                Self.Attack();
                attackTimer -= Self.AttackDelay;
            }
        }
    }
}