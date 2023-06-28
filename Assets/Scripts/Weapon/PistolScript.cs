using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : WeaponBase
{
    protected override void OnGunTriggerPull()
    {
        Fire();
    }
}