using System;
using UnityEngine;

[Serializable]
public class FacilityData
{
    [Header("Body")]
    public int MaxHp;
    public int Hp;

    [Header("AI")]
    public LayerMask DetectTargetLayer;
    /// <summary>
    /// AI가 적을 공격할 수 있다 판단하는 거리.
    /// </summary>
    public float AttackTargetRange = 10f;
    public float TargetMissingRange = 10f; // 타겟이 된 대상의 타겟팅을 해제하는 거리
    public float TargetMissingDelay = 1f;  // 적이 사라졌을 때 적대 상태를 유지하는 시간
    public float DetectRange = 10f;      // 적을 최초 감지하는 거리
    public float DetectDelay = 1f;


    [Header("Status")]
    // 공격 관련
    public int AttackDamage = 1;  // 공격력
    public float AttackDelay = 1f;   // 공격과 공격 사이의 딜레이
    public float AttackSpeed = 1f;   // 공격 1회의 속도
    public float AttackRange = 1f;   // 공격 범위
    public float BulletSpeed = 1f;

    // 이동 관련
    public float MoveSpeed = 0;    // 평상시 이동속도

    public BulletData Bullet;

    public FacilityData() { }
    public FacilityData(FacilityData data)
    {
        MaxHp = data.MaxHp;
        Hp = data.Hp;
        DetectTargetLayer = data.DetectTargetLayer;
        AttackTargetRange = data.AttackTargetRange;
        TargetMissingRange = data.TargetMissingRange;
        TargetMissingDelay = data.TargetMissingDelay;
        DetectRange = data.DetectRange;
        DetectDelay = data.DetectDelay;
        AttackDamage = data.AttackDamage;
        AttackDelay = data.AttackDelay;
        AttackSpeed = data.AttackSpeed;
        AttackRange = data.AttackRange;
        BulletSpeed = data.BulletSpeed;
        MoveSpeed = data.MoveSpeed;
        Bullet = data.Bullet;
    }
}