using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponData Data;
    public GameObject Owner;
    public Transform GunPointTr;

    [SerializeField]
    [Min(0)]
    private int _ammoCount;     // 장전된 총알 개수
    public int AmmoCount
    {
        get
        {
            return _ammoCount;
        }
        set
        {
            _ammoCount = value;
            if (_ammoCount < 0)
            {
                _ammoCount = 0;
            }
        }
    }
    public bool IsAmmoEmpty => _ammoCount == 0; // 총알이 텅 비었는가?
    private float _cooldown;    // 남은 쿨타임
    public float Cooldown
    {
        get
        {
            return _cooldown;
        }
        set
        {
            _cooldown = value;
            if (_cooldown < float.Epsilon)
            {
                _cooldown = 0f;
            }
        }
    }
    public bool IsCooldownDone => _cooldown < float.Epsilon;    // 쿨타임이 다 돌았는가?

    private GunTriggerState TriggerState = GunTriggerState.Off;

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }
    public virtual void OnFixedUpdate()
    {
        Cooldown -= Time.fixedDeltaTime;
        if(TriggerState == GunTriggerState.On)
            OnGunTriggerDuring();
    }

    // Inherance Method
    protected virtual void FireLogic()
    {
        CreateNewBullet();
    }
    protected virtual void OnGunTriggerPull() { }
    protected virtual void OnGunTriggerRelease() { }
    protected virtual void OnGunTriggerDuring() { }

    // Inner Method
    protected void SetAmmoFull()
    {
        AmmoCount = Data.MaxAmmoAmount;
    }


    // Interface Method
    public virtual void Init(WeaponData data, GameObject owner)
    {
        this.Data = data;
        this.Owner = owner;
    }

    /// <summary>
    /// 총 발사 명령 메서드. 발사 성공 시 true 반환, 실패 시 false 반환
    /// </summary>
    /// <returns></returns>
    public virtual bool Fire()
    {
        if (IsCooldownDone)
        {
            FireLogic();
            RecountCooldown();
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void Reload() { }
    public void SetTrigger(bool onoff)
    {
        if (TriggerState == GunTriggerState.Off && onoff == true)
        {
            TriggerState = GunTriggerState.On;
            OnGunTriggerPull();
        }
        else if (TriggerState == GunTriggerState.On && onoff == false)
        {
            TriggerState = GunTriggerState.Off;
            OnGunTriggerRelease();
        }
    }
    public void RecountCooldown()
    {
        Cooldown = Data.Cooldown;
    }

    /// <summary>
    /// 총알을 생성하고 초기화하는 메서드
    /// </summary>
    /// <returns></returns>
    public BulletBase CreateNewBullet()
    {
        var bullet = ProduceBullet();
        Vector3 angle = bullet.transform.eulerAngles;
        Vector2 random = UnityEngine.Random.insideUnitCircle * Data.BulletSpread;
        angle.x += random.x;
        angle.y += random.y;
        bullet.transform.eulerAngles = angle;
        return bullet;
    }

    public BulletBase ProduceBullet()
    {
        Debug.Log(Data.Bullet.speed);
        var bullet = EntityManager.Instance.CreateBullet(Data.Bullet, GunPointTr);
        return bullet;
    }
}

[Serializable]
public class WeaponData
{
    public BulletData Bullet;
    public int Damage = 1;
    public float Cooldown = 1f;
    public float BulletSpread = 0f;
    public int MaxAmmoAmount;
    public float ReloadCooltime = 1.2f;
}

public enum GunFireMode
{
    Auto,
    Single,
    Rapid,
}

public enum GunTriggerState
{
    On,
    Off,
}