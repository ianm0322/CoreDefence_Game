using UnityEngine;

public class EnemyState_Move : EnemyState
{
    public EnemyState_Move(StateMachine machine) : base("Move", machine)
    {
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
        if (DetectTarget())
        {
            Self.MoveState("Targeting");
            return;
        }
    }

    public override void OnStateEnter(IState state)
    {
        Self.Agent.SetDestination(StageInfoManager.Instance.CorePosition);
        Self.Agent.speed = Self.data.MoveSpeed;
    }

    protected virtual bool DetectTarget()
    {
        var target = Self.Scanner.ScanEntity();
        if (target != null)
        {
            Self.FocusTarget = (target.transform);
            return true;
        }
        else
        {
            Self.FocusTarget = (null);
            return false;
        }
    }
}