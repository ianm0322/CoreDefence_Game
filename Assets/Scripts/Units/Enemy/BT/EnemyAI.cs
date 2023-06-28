using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
using static BT.NodeHelper;
using static BT.Unity.NodeHelperForUnity;
using static CTType;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour, IBehaviorTree, IEnemyController
{
    protected EnemyBody _body;
    protected Rigidbody _rigidbody;
    protected NavMeshAgent _agent;
    protected Collider _collider;

    RootNode _root;
    public EnemyKind Kind;
    public EnemyData Data;
    public Transform Target { get; set; }

    public EntitySelector Scanner;

    protected virtual void Awake()
    {
        TryGetComponent(out _body);
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _agent);
        TryGetComponent(out _collider);
        _root = GenerateBT();
    }

    protected virtual void Update()
    {
        Operate();
    }

    public abstract RootNode GenerateBT();

    public void Operate()
    {
        _root.Evaluate();
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