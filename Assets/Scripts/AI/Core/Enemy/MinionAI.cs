using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BT.NodeHelper;

public class MinionAI : EnemyAI, ILateUpdateListener
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

                // Target is exist pattern:
                Sequence(
                    // Check target is exist
                    new IsTargetExistNode(this),
                    Select(
                        // Sequence: Escape if target is out of range
                        Sequence(
                            new TimeOverNode(Data.DetectDelay, new CheckTargetOutOfRangeNode(this.transform, this, AIInfo)),
                            new SetTargetNullNode(this)
                            ),
                        // Sequence: Attack
                        Sequence(
                            new CheckAttackableReachNode(this),     // 공격 가능 위치면
                            new AgentStopNode(Agent),               // 에이전트 멈추고
                            new Minion_AttackNode(this)             // 공격
                            ),
                        // Move to target
                        new ChaseTargetNode(this)
                        )
                    ),

                // Target is non-exist pattern
                new SetTargetNode(this),

                // Move to core
                new ChaseTargetNode(this, StageInfoManager.Instance.Core.transform)
                )
            );
    }

    public void OnLateUpdate()
    {
        if (!Body.IsDied)
        {
            LookTarget();
            Anim.SetFloat("MoveVelocity", Agent.desiredVelocity.magnitude * 0.25f);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        //Scanner = new EntitySelector(
        //    new SphereScanner(transform, Data.DetectRange, AIInfo.DetectTargetLayer),
        //    new EntityClassifier_MeleeAgent(transform, Agent, AIInfo.DetectTargetTags)
        //    );
    }

    protected override void Start()
    {
        base.Start();
        StartBT();
    }

    private void LookTarget()
    {
        if (Target != null)
        {
            Vector3 look = (Target.transform.position - transform.position).normalized;
            look.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, look, 5 * Time.deltaTime);
        }
    }

    public override void InitForInstantiate()
    {
        base.InitForInstantiate();
    }

    public override void OnCreateFromPool(object dataObj)
    {
        base.OnCreateFromPool(dataObj);

        _selector = new MinionTargetSelector(this);
    }
}
