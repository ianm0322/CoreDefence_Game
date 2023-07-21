using System;
using UnityEngine;

public class EnemyState_RobotAttack : EnemyState
{
    RobotAIController robot;
    private string state_wait = "RobotWait";

    float _attackTimer;
    float _chaseTimer;

    public EnemyState_RobotAttack(StateMachine self) : base("RobotAttack", self)
    {
        robot = self as RobotAIController;
    }

    public override void OnStateEnter(IState state)
    {
        base.OnStateEnter(state);

        _attackTimer = 0f;
        _chaseTimer = 0f;
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        AttackUpdate();
        MoveToCoreUpdate();

        if (CanChase()) Self.MoveState(state_wait);
    }

    private void LookTarget()
    {
        Vector3 tAngle;
        robot.BodyTr.LookAt(Self.FocusTarget);
        tAngle = robot.BodyTr.eulerAngles;
        tAngle.x = 0f;
        tAngle.z = 0f;
        robot.BodyTr.eulerAngles = tAngle;
    }

    // 타겟이 추적 가능(1. 가깝고 2. 레이를 쐈을 때 장애물이 없어야 함)한지 검사함.
    private bool CanChase()
    {
        if (IsDistTooFar()) // 너무 멀면 x
            return false;
        if (!NoObstacleBetween()) // 사이에 장애물이 있으면 x
            return false;
        return true;
    }

    private void AttackUpdate()
    {
        _attackTimer += Time.deltaTime * Self.data.AttackSpeed;
        if(_attackTimer > 1f)   // 1초마다 1회 공격
        {
            Self.Attack();
            _attackTimer -= 1f;
        }
    }

    private void MoveToCoreUpdate()
    {
        Self.Agent.SetDestination(StageInfoManager.Instance.CorePosition);
    }

    private bool IsDistTooFar()
    {
        if (MathUtility.CompareDist(Self.FocusTarget.position - Self.transform.position, Self.data.AttackTargetRange) > 0)    // 너무 멀면 추적 불가
        {
            _chaseTimer += Time.deltaTime;
            if (_chaseTimer >= Self.data.TargetMissingDelay)
                return true;
        }
        else
        {
            _chaseTimer = 0f;
        }
        return false;
    }

    private bool NoObstacleBetween()
    {
        return !Self.Scanner.CheckScanned(Self.FocusTarget);
    }
}
