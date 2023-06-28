using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : BaseState
{
    public EnemyAIController Self;

    public EnemyState(string type, StateMachine machine) : base(type, machine)
    {
        Self = (EnemyAIController)machine;
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
    }
}
