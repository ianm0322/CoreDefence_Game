using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTType : MonoBehaviour
{
    [Flags]
    public enum PlayerControllerStateType
    {
        Nothing     = 0,
        OnGround    = 1,
        InAir       = 2,
        Jump        = 4,
        Everything  = -1
    }

    public enum EnemyKind
    {
        None            = -1,
        Minion          ,
        Robot,
        Golem,
        End
    }

    public enum EntityKind
    {
        WEAPON_ID = 100,
        Pistol,
        Rifle,
        Grenade,
        W3,
        W4,
        W5,

        FACILITY_ID = 200,
        ShooterTurret,
        CannonTurret,

        BULLET_ID = 300,
        NormalBullet,
        GravityBullet,
        GrenadeBullet,

        ENEMY_ID = 400,
        Minion,
        Robot,
        Golem,
    }
}

public interface IUpdateListener
{
    void OnUpdate();
}

public interface IFixedUpdateListener
{
    void OnFixedUpdate();
}

public interface ILateUpdateListener
{
    void OnLateUpdate();
}