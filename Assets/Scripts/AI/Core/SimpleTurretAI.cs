using BT;
using static BT.NodeHelper;
using UnityEngine;

public class SimpleTurretAI : FacilityAI, IShooter, ITargetter
{
    public Transform GunTr;
    public Transform HeadTr;

    public Transform Target { get; set; }
    public EntitySelector Scanner { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override RootNode MakeBT()
    {
        return Root(
            Select(
                new IsParalysisNode(this),
                Sequence(
                    new CountdownNode(Data.AttackCount).SetOption(false),
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