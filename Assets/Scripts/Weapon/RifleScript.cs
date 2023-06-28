using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleScript : WeaponBase
{
    [SerializeField]
    WeaponState state = WeaponState.Default;
    Coroutine ReloadCor;

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
        yield return new WaitForSeconds(1.2f);
        SetAmmoFull();
        state = WeaponState.Default;
        ReloadCor = null;
    }
}

[System.Flags]
[System.Serializable]
public enum WeaponState
{
    None                    = 0,
    Default                 = 1,
    Firing                  = 2,
    Reloading               = 4,
    Jammed                  = 8,
    NoAmmo                  = 16,

    Everything = -1,
    CanFireState = Firing | Default,
    CantFireState = Reloading | Jammed | NoAmmo,
}