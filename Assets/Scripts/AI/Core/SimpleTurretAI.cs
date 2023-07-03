using BT;
using static BT.NodeHelper;
using UnityEngine;

public class SimpleTurretAI : FacilityAI, IShooter
{
    public Transform GunTr;

    public override RootNode MakeBT()
    {
        return Root(
            Select(
                new IsParalysisNode(this),
                Sequence(
                        new CountdownNode(3).SetOption(false),
                        new SimpleShootingNode(this),
                        new WaitForAttackSpeedNode(this.Data)
                    ),
                    new WaitForAttackDelayNode(this.Data)
                )
            );
    }

    public void Shot()
    {
        EntityManager.Instance.CreateBullet(Data.Bullet, GunTr);
    }
}