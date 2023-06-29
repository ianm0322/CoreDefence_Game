using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [Header("Body")]
    public int MaxHp;
    public int Hp;

    [Header("AI")]
    public LayerMask DetectTargetLayer;
    /// <summary>
    /// AI가 적을 공격할 수 있다 판단하는 거리.
    /// </summary>
    public float AttackTargetRange;  // 
    public float TargetMissingRange; // 타겟이 된 대상의 타겟팅을 해제하는 거리
    public float TargetMissingDelay;  // 적이 사라졌을 때 적대 상태를 유지하는 시간
    public float DetectRange;      // 적을 최초 감지하는 거리
    public float DetectDelay = 0f;

    [Header("Agent")]
    public float MoveSpeed = 10;    // 평상시 이동속도
    public float AttackSpeedMultiple = 0.8f;    // 공격 중 이동속도 배율
    public float StopDistance => AttackTargetRange;
    public float MoveOnAttckSpeed => MoveSpeed * AttackSpeedMultiple;

    [Header("Status")]
    // 공격 관련
    public float AttackDelay;   // 공격과 공격 사이의 딜레이
    public float AttackSpeed;   // 공격 1회의 속도
    public int AttackDamage;  // 공격력
    public float AttackRange;   // 공격 범위
    public float BulletSpeed;

    public BulletData Bullet;

    public EnemyData() { }
    public EnemyData(EnemyData data)
    {
        AttackTargetRange = data.AttackTargetRange;
        TargetMissingRange = data.TargetMissingRange;
        DetectRange = data.DetectRange;
        TargetMissingDelay = data.TargetMissingDelay;

        MoveSpeed = data.MoveSpeed;
        AttackSpeedMultiple = data.AttackSpeedMultiple;

        AttackDelay = data.AttackDelay;
        AttackSpeed = data.AttackSpeed;
        AttackDamage = data.AttackDamage;
        AttackRange = data.AttackRange;
        BulletSpeed = data.BulletSpeed;
    }
}
