using UnityEngine;

[System.Serializable]
public class EntityData
{
    [Header("Body")]
    public int MaxHp;
    public int Hp;
    public int MaxFocusCount = 3;

    [Header("Agent")]
    public float MoveSpeed = 10;    // 평상시 이동속도
    public float AttackSpeedMultiple = 0.8f;    // 공격 중 이동속도 배율

    [Header("Status")]
    // 공격 관련
    public float AttackDelay;   // 공격과 공격 사이의 딜레이
    public float AttackSpeed;   // 공격 1회의 속도
    public int AttackCount;   // 공격 1회의 속도
    public int AttackDamage;  // 공격력
    public float AttackRange;   // 공격 범위
    public float BulletSpeed;

    public EntityData() { }
    public EntityData(EntityData data)
    {
        MaxHp = data.MaxHp;
        Hp = data.Hp;
        MaxFocusCount = data.MaxFocusCount;
        AttackDamage = data.AttackDamage;
        AttackDelay = data.AttackDelay;
        AttackSpeed = data.AttackSpeed;
        AttackRange = data.AttackRange;
        BulletSpeed = data.BulletSpeed;
        MoveSpeed = data.MoveSpeed;
        AttackCount = data.AttackCount;
    }
}