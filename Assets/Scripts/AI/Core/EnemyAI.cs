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
    [field: SerializeField]
    public Transform Target { get; set; }

    public EntitySelector Scanner { get; set; }

    private Coroutine _dieCoroutine;

    protected virtual void Start()
    {
        Body.OnDiedEvent += OnDied;
    }

    protected virtual void Update()
    {
    }

    public virtual void InitForInstantiate()
    {
    }

    public virtual void OnUpdate()
    {
        if (!Body.IsDied)
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

        if (_dieCoroutine != null)
        {
            StopCoroutine(_dieCoroutine);
            _dieCoroutine = null;
        }
    }

    protected virtual void OnDied()
    {
        Anim.SetBool("IsDied", true);
        Agent.enabled = false;
        Collider.enabled = false;

        _dieCoroutine = StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(5f);
        EntityManager.Instance.DestroyEnemy(this);
        _dieCoroutine = null;
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

        // Anim
        Anim.SetFloat("AttackSpeed", Data.AttackSpeed);
    }
}