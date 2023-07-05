using BT;
using static BT.NodeHelper;
using UnityEngine;

public class CannonTurretAI : FacilityAI, IShooter
{
    public Transform GunTr;
    public Transform HeadTr;

    protected override void Awake()
    {
        base.Awake();
        TestUpdateManager.Instance.update.Add(this.gameObject);
        Scanner = new EntitySelector(
            new SphereScanner(HeadTr, AIInfo.DetectRange, AIInfo.DetectTargetLayer),
            new EntityClassifier_RayClassifier(HeadTr, AIInfo.DetectTargetTags)
            );
    }

    public override RootNode MakeBT()
    {
        return Root(
            Select(
                new IsParalysisNode(this),

                Sequence(   // Target is exist
                    new IsTargetExistNode(this),
                    new LookAtTargetNode(this, HeadTr, new Vector3(0, -90, 5), new Vector3(1, 1, -1), AxisOrder.ZYX),

                    Select(
                        new SimpleTurretShootingNode(this, Data),
                        Sequence(   // Target out of range sequence
                            new CheckTargetOutOfRangeNode(this.transform, this, AIInfo),
                            new SetTargetNullNode(this)
                            )
                        )
                    ),
                new SetTargetNode(this)
                )
            );
    }

    public void Shot()
    {
        EntityManager.Instance.CreateBullet(Data.Bullet, GunTr);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
