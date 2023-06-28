using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAI : EnemyAI
{
    public LayerMask TargetLayer;
    public string[] TargetTags;

    private float _attackTimer;

    public override RootNode GenerateBT()
    {
        throw new System.NotImplementedException();
    }

    protected override void Awake()
    {
        base.Awake();
        Scanner = new EntitySelector(
            new SphereScanner(transform, Data.DetectRange, TargetLayer),
            new EntityClassifier_MeleeAgent(transform, _agent, TargetTags)
            );
    }
}
