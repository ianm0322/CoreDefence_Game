using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_MassiveHit : EnemyState
{
    public EnemyState_MassiveHit(StateMachine machine) : base("MassiveHit", machine)
    {
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);

        Self.Agent.enabled = false;
        Self.Rigid.isKinematic = false;
    }

    public override void OnStateExit(IState state)
    {
        base.OnStateExit(state);

        Self.Agent.enabled = true;
        Self.Rigid.isKinematic = true;
    }
}
