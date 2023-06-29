using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BT.NodeHelper;
using static BT.Unity.NodeHelperForUnity;

public class MinionAI : EnemyAI
{
    public string DebugState;

    public LayerMask TargetLayer;
    public string[] TargetTags;

    public override RootNode MakeBT()
    {
        return Root(
            Select(
                Sequence(   // Ÿ���� ������
                    new SetTargetNode(this),
                    UntilSuccess(
                        new SimpleParallel(
                            LogResult(new CheckTargetOutOfRangeNode(this)),   // Ÿ�ٿ��� ����� Ż��
                            Select( // �����ϰų�, �̵��ϰų�
                                Sequence(
                                    new CheckAttackableReachNode(this),     // ���� ���� ��ġ��
                                    new AgentStopNode(Agent),               // ������Ʈ ���߰�
                                    new Minion_AttackNode(this),             // ����
                                    Wait(1f)
                                    ),
                                new ChaseTargetNode(this)
                                )
                            ).SetOption(runSubOnFail: true)
                        )
                    ),
                Sequence(   // �ھ�� �̵� ������
                    Call(() => Target = null),
                    new ChaseTargetNode(this, GameManager.Instance.Core.transform)
                    )
                )
            );
    }

    public void SetDebugState(string str)
    {
        if (DebugState != str)
        {
            DebugState = str;
            Debug.Log("State Change : " + str);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Scanner = new EntitySelector(
            new SphereScanner(transform, Data.DetectRange, TargetLayer),
            new EntityClassifier_MeleeAgent(transform, Agent, TargetTags)
            );
    }

    private void Start()
    {
        StartBT();
    }

    private void LateUpdate()
    {
        LookTarget();
    }

    private void LookTarget()
    {
        if(Target != null)
        {
            Vector3 look = (Target.position - transform.position).normalized;
            look.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, look, 5 * Time.deltaTime);
        }
    }
}