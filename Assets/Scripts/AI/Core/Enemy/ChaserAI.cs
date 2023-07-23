using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BT.NodeHelper;

public class ChaserAI : EnemyAI, ILateUpdateListener
{
    public string DebugState;

    public override RootNode MakeBT()
    {
        return Root(
            Select(
                Sequence(
                    new IsDiedNode(Body),
                    new EnemyDieNode(this)
                    ),

                new IsParalysisNode(this),

                Select(
                    )
                )
            );
    }

    public void OnLateUpdate()
    {
        if (!Body.IsDied)
        {
            Anim.SetFloat("MoveVelocity", Agent.desiredVelocity.magnitude * 0.25f);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _selector = new EntitySelector(
            new SphereScanner(transform, Data.DetectRange, AIInfo.DetectTargetLayer),
            new EntityClassifier_MeleeAgent(transform, Agent, AIInfo.DetectTargetTags)
            );
    }

    protected override void Start()
    {
        base.Start();
        StartBT();
    }
}