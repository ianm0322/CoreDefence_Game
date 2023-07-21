using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BT.NodeHelper;
using static BT.Unity.NodeHelperForUnity;

public class RobotAI : EnemyAI, ILateUpdateListener, IShooter
{
    public string DebugState;

    public Transform[] FirePointTr;
    public Transform BodyTr;
    public Transform GunTr;

    public override RootNode MakeBT()
    {
        return Root(
            Select(
                Sequence(
                    new IsDiedNode(Body),
                    new EnemyDIeNode(this)
                    ),

                new IsParalysisNode(this),

                Sequence(
                    // Default: move to core
                    new ChaseTargetNode(this, StageInfoManager.Instance.Core.transform),

                    Select(
                        // Target is exist sequence:
                        Sequence(
                            new IsTargetExistNode(this),
                            new RobotShootingNode(this),
                            Success(Sequence(
                                // Escape
                                new TimeOverNode(Data.TargetMissingDelay, new CheckTargetOutOfRangeNode(this.transform, this, AIInfo)),
                                new SetTargetNullNode(this)
                                ))
                            ),

                        new SetTargetNode(this).SetOption(true)
                        )
                    )
                )
            );
    }

    protected override void Awake()
    {
        base.Awake();
        Scanner = new EntitySelector(
             new SphereScanner(FirePointTr[0], Data.DetectRange, Data.DetectTargetLayer),
             new EntityClassifier_Robot(transform, new string[2] { "Player", "Facility" })
            );
    }

    protected override void Start()
    {
        base.Start();
        StartBT();
    }

    //// TEST
    //protected override void Update()
    //{
    //    base.Update();
    //    OnUpdate();
    //}

    //private void LateUpdate()
    //{
    //    OnLateUpdate();
    //}
    //// end TEST

    public void OnLateUpdate()
    {
        if (!Body.IsDied)
        {
            LookTarget();
            Anim.SetFloat("MoveVelocity", Agent.desiredVelocity.magnitude * 0.25f);
        }
    }

    public void Shot()
    {
        EntityManager.Instance.CreateBullet(Data.Bullet, FirePointTr[0].position, Quaternion.LookRotation(Target.position - FirePointTr[0].position));
        EntityManager.Instance.CreateBullet(Data.Bullet, FirePointTr[1].position, Quaternion.LookRotation(Target.position - FirePointTr[1].position));
    }

    private void LookTarget()
    {
        if (Target)
        {
            Vector3 angle;

            Quaternion rotation = Quaternion.LookRotation(Target.position - BodyTr.position);
            angle = Vector3.zero;
            angle.y = rotation.eulerAngles.y - 90;
            BodyTr.eulerAngles = angle;

            rotation = Quaternion.LookRotation(Target.position - GunTr.position);
            angle = rotation.eulerAngles;
            angle = Vector3.zero;
            angle.z = -rotation.eulerAngles.x;
            GunTr.localEulerAngles = angle;
        }
        else
        {
            BodyTr.localEulerAngles = Vector3.zero;
            GunTr.localEulerAngles = Vector3.zero;
        }
    }
}
