using UnityEngine;

public class EnemyState_RobotWait : EnemyState
{
    private string state_move = "RobotMove";
    private string state_attack = "RobotAttack";

    private float _startTime = 0;
    private float _duration = 0;

    public EnemyState_RobotWait(StateMachine self) : base("RobotWait", self)
    {
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);
        _duration = Self.data.TargetMissingDelay;
        _startTime = Time.time;
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        // 일정 시간 후 타겟 해제
        // 
        if (IsTimeover())
        {
            Self.MoveState(state_move);
        }
        else if (CanRetarget())
        {
            //MyDebug.Log(Self.Scanner.CheckScanned(Self.FocusTarget));
            Self.MoveState(state_attack);
        }
    }

    private bool CanRetarget()
    {
        return Self.Scanner.CheckScanned(Self.FocusTarget);
    }

    private bool IsTimeover()
    {
        return (Time.time - _startTime) > _duration;
    }
}
