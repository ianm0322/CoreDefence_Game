using BT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacilityController : IPoolingObject
{

}

public abstract class FacilityAI : BehaviorTree, IFacilityController
{
    public void InitForInstantiate()
    {

    }

    public void OnCreateFromPool(object dataObj)
    {
    }

    public void OnPushToPool()
    {
    }
}

[Serializable]
public class FacilityData
{
    [Header("Body")]
    public int MaxHp;
    public int Hp;

    [Header("AI")]
    public LayerMask DetectTargetLayer;
    /// <summary>
    /// AI�� ���� ������ �� �ִ� �Ǵ��ϴ� �Ÿ�.
    /// </summary>
    public float AttackTargetRange = 10f;
    public float TargetMissingRange = 10f; // Ÿ���� �� ����� Ÿ������ �����ϴ� �Ÿ�
    public float TargetMissingDelay = 1f;  // ���� ������� �� ���� ���¸� �����ϴ� �ð�
    public float DetectRange = 10f;      // ���� ���� �����ϴ� �Ÿ�
    public float DetectDelay = 1f;


    [Header("Status")]
    // ���� ����
    public int AttackDamage = 1;  // ���ݷ�
    public float AttackDelay = 1f;   // ���ݰ� ���� ������ ������
    public float AttackSpeed = 1f;   // ���� 1ȸ�� �ӵ�
    public float AttackRange = 1f;   // ���� ����
    public float BulletSpeed = 1f;

    // �̵� ����
    public float MoveSpeed = 0;    // ���� �̵��ӵ�

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