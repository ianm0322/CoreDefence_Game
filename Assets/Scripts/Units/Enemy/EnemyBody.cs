using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBody : CD_GameObject
{
    // Data

    protected virtual void Awake()
    {
        NavMeshAgent agent;
        TryGetComponent(out agent);
    }
}