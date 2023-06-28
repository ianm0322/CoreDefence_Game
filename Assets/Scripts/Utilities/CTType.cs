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