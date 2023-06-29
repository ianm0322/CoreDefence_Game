using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using static CTType;
using UnityEngine.AI;

public abstract class EnemyAI : BehaviorTree, IEnemyController, IUpdateListener
{
    [HideInInspector]
    public CD_GameObject Body;
    [HideInInspector]
    public Rigidbody Rigidbody;
    [HideInInspector]
    public NavMeshAgent Agent;
    [HideInInspector]
    public Collider Collider;
    [HideInInspector]
    public Animator Anim;
    
    public EnemyKind Kind;
    public EnemyData Data;
    [field: SerializeField]
    public Transform Target { get; set; }

    public EntitySelector Scanner;

    private Coroutine _dieCoroutine;

    protected virtual void Awake()
    {
        TryGetComponent(out Body);
        TryGetComponent(out Rigidbody);
        TryGetComponent(out Agent);
        TryGetComponent(out Collider);
        TryGetComponent(out Anim);
    }

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
    }

    public virtual void OnPushToPool()
    {
        gameObject.SetActive(false);
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