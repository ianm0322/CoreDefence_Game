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
    public Collider Target { get; set; }

    //public EntitySelector Scanner { get; set; }
    // 셀렉터테스트
    protected ITargetSelector _selector;

    protected virtual void Start()
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
    /// EnemyData를 현재 상태에 반영하는 메서드.
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

    public ITargetSelector GetTargetSelector()
    {
        return _selector;
    }

    public Collider GetTarget()
    {
        return Target;
    }

    public void SetTarget(Collider target)
    {
        this.Target = target;
    }
}