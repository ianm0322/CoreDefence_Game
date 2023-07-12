using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleScript : WeaponBase
{
    [SerializeField]
    WeaponState state = WeaponState.Default;
    Coroutine ReloadCor;

    private WaitForSeconds _reloadTime;

    protected void Start()
    {
        new WaitForSeconds(Data.ReloadCooltime);
    }

    protected override void OnGunTriggerDuring()
    {
        if (!IsAmmoEmpty && (state & WeaponState.CanFireState) != 0)
        {
            if(Fire()) AmmoCount--;
            state = WeaponState.Firing;
        }
        if (IsAmmoEmpty)
        {
            state = WeaponState.NoAmmo;
        }
    }

    protected override void OnGunTriggerRelease()
    {
        if (state == WeaponState.Firing)
            state = WeaponState.Default;
    }

    public override void Reload()
    {
        Debug.Log("Reloading!");
        if (state != WeaponState.Firing)
        {
            ReloadCor = StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return null;
        state = WeaponState.Reloading;
        yield return _reloadTime;
        SetAmmoFull();
        state = WeaponState.Default;
        ReloadCor = null;
    }
}
