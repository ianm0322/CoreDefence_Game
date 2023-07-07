using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using static CTType;
using UnityEngine.AI;

public abstract class EnemyAI : AIController, IEnemyController, IUpdateListener, ITargetter
{
    public EnemyKind Kind;
    public EnemyData Data;
    public AIData AIInfo;
    [field: SerializeField]
    public Transform Target { get; set; }

    public EntitySelector Scanner { get; set; }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public virtual void InitForInstantiate()
    {
    }

    public virtual void OnUpdate()
    {
        Operate();
    }

    public virtual void OnCreateFromPool(object dataObj)
    {
        EnemyData data = dataObj as EnemyData;
        if (data != null)
        {
            Data = data;
        }

        if (Root == null)
            StartBT();

        // SetData
        Anim.SetBool("IsDied", false);
        Agent.isStopped = false;
        Collider.enabled = true;
        gameObject.SetActive(true);

        ApplyDataUpdate();
    }

    public virtual void OnPushToPool()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// EnemyData�� ���� ���¿� �ݿ��ϴ� �޼���.
    /// </summary>
    protected virtual void ApplyDataUpdate()
    {
        // Agent
        Agent.speed = Data.MoveSpeed;

        // Body
        Body.MaxHp = Data.MaxHp;
        Body.Hp = Data.Hp;
        Body.MaxFocusCount = Data.MaxFocusCount;

        // Anim
        Anim.SetFloat("AttackSpeed", Data.AttackSpeed);
    }
}