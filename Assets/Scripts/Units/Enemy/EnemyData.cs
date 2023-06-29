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
    /// AI�� ���� ������ �� �ִ� �Ǵ��ϴ� �Ÿ�.
    /// </summary>
    public float AttackTargetRange;  // 
    public float TargetMissingRange; // Ÿ���� �� ����� Ÿ������ �����ϴ� �Ÿ�
    public float TargetMissingDelay;  // ���� ������� �� ���� ���¸� �����ϴ� �ð�
    public float DetectRange;      // ���� ���� �����ϴ� �Ÿ�
    public float DetectDelay = 0f;

    [Header("Agent")]
    public float MoveSpeed = 10;    // ���� �̵��ӵ�
    public float AttackSpeedMultiple = 0.8f;    // ���� �� �̵��ӵ� ����
    public float StopDistance => AttackTargetRange;
    public float MoveOnAttckSpeed => MoveSpeed * AttackSpeedMultiple;

    [Header("Status")]
    // ���� ����
    public float AttackDelay;   // ���ݰ� ���� ������ ������
    public float AttackSpeed;   // ���� 1ȸ�� �ӵ�
    public int AttackDamage;  // ���ݷ�
    public float AttackRange;   // ���� ����
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
