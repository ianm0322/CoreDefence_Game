using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using static CTType;
using UnityEngine.AI;

public abstract class EnemyAI : BehaviorTree
{
    public EnemyBody Body;
    public Rigidbody Rigidbody;
    public NavMeshAgent Agent;
    public Collider Collider;
    
    public EnemyKind Kind;
    public EnemyData Data;
    public Transform Target { get; set; }

    public EntitySelector Scanner;

    protected virtual void Awake()
    {
        TryGetComponent(out Body);
        TryGetComponent(out Rigidbody);
        TryGetComponent(out Agent);
        TryGetComponent(out Collider);
    }

    protected virtual void Update()
    {
        Operate();
    }

    public virtual void InitForInstantiate()
    {
    }

    public virtual void OnCreateFromPool(object dataObj)
    {
        EnemyData data = dataObj as EnemyData;
        if (data != null)
        {
            Data = new EnemyData(data);
        }
    }

    public virtual void OnPushToPool()
    {
        gameObject.SetActive(false);
    }
}