using BT;
using static BT.NodeHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurretAI : FacilityAI, IShooter
{
    public Transform GunTr;

    public override RootNode MakeBT()
    {
        return Root(
                new SimpleShootingNode(this)
            );
    }

    public void Shot()
    {
        EntityManager.Instance.CreateBullet(Data.Bullet, GunTr);
    }
}
