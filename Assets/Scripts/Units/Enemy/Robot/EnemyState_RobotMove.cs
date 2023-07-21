using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_RobotMove : EnemyState
{
    private string state_attack = "RobotAttack";

    public EnemyState_RobotMove(StateMachine self) : base("RobotMove", self)
    {

    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);

        Self.FocusTarget = null;
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        MoveToCoreUpdate();
        if (DetectTarget()) Self.MoveState(state_attack);
    }

    private bool DetectTarget()
    {
        Self.FocusTarget = Self.Scanner.ScanEntity();
        if(Self.FocusTarget != null)
        {
            return true;
        }
        return false;
    }

    private void MoveToCoreUpdate()
    {
        Self.Agent.SetDestination(StageInfoManager.Instance.CorePosition);
    }
}
