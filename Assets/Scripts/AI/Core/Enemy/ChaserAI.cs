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
                    new EnemyDIeNode(this)
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
                            new CheckAttackableReachNode(this),     // ���� ���� ��ġ��
                            new AgentStopNode(Agent),               // ������Ʈ ���߰�
                            new Minion_AttackNode(this)             // ����
                            ),
                        // Move to target
                        new ChaseTargetNode(this)
                        )
                    ),

                // Target is non-exist pattern
                new SetTargetNode(this),

                // Move to core
                new ChaseTargetNode(this, GameManager.Instance.Core.transform)
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
        Scanner = new EntitySelector(
            new SphereScanner(transform, Data.DetectRange, AIInfo.DetectTargetLayer),
            new EntityClassifier_MeleeAgent(transform, Agent, AIInfo.DetectTargetTags)
            );
    }

    protected override void Start()
    {
        base.Start();
        StartBT();
    }