using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : WeaponBase
{
    private float _powerTimer;
    private float _power => Mathf.Clamp01((Time.time - _powerTimer) / MaxCharging);

    public float MaxCharging = 1f;

    protected override void OnGunTriggerPull()
    {
        base.OnGunTriggerPull();

        _powerTimer = Time.time;
    }

    protected override void OnGunTriggerRelease()
    {
        base.OnGunTriggerRelease();

        if (!IsAmmoEmpty)
        {
            float baseSpeed = Data.Bullet.speed;
            Data.Bullet.speed = baseSpeed * _power;
            if (Fire()) AmmoCount--;
            Data.Bullet.speed = baseSpeed;
        }
    }
}
