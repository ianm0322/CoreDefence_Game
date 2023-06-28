using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BT.NodeHelper;

public class MinionAI : EnemyAI
{
    public LayerMask TargetLayer;
    public string[] TargetTags;

    private float _attackTimer;

    public override RootNode GenerateBT()
    {
        return Root(
            Select(
                Sequence(
                    new SetTargetNode(this),
                    UntilFail(
                        Sequence(
                            
                            )
                        )
                    ),
                Sequence(
                    Call(()=>Target = GameManager.Instance.Core.transform),
                    new ChaseTarget(this)
                    )
                )
            );
    }

    protected override void Awake()
    {
        base.Awake();
        Scanner = new EntitySelector(
            new SphereScanner(transform, Data.DetectRange, TargetLayer),
            new EntityClassifier_MeleeAgent(transform, Agent, TargetTags)
            );
    }
}
