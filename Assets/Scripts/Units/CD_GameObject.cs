using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CD_GameObject : MonoBehaviour
{
    public int Hp;
    public int MaxHp;
    public int FocusCount { get; private set; } = 0;
    public int MaxFocusCount = 3;

    public bool CanFocus => FocusCount < MaxFocusCount; // 임시 지정

    public bool IsDied { get; private set; } = false;
    public event Action OnDiedEvent;


    public int GetHp()
    {
        return Hp;
    }

    public void SetHp(int hp)
    {
        if(hp > MaxHp)
        {
            this.Hp = MaxHp;
        }
        else if(hp > 0)
        {
            this.Hp = hp;
        }
        else
        {
            this.Hp = 0;
            Die();
        }
    }

    public virtual void GiveDamage(int damage)
    {
        //Debug.Log($"{name} take damage ({damage}) : {GetHp()} => {GetHp()-damage}");
        SetHp(GetHp() - damage);
    }

    public virtual void Die()
    {
        IsDied = true;
        //OnDiedEvent?.Invoke();
    }
    public virtual void Init(EntityData data)
    {
        IsDied = false;
        MaxHp = data.MaxHp;
        Hp = data.Hp;
    }

    public bool AddFocus()
    {
        if (CanFocus)
        {
            FocusCount++;
            return true;    
        }
        else
        {
            return false;
        }
    }

    public void ReleaseFocus()
    {
        if(FocusCount > 0)
        {
            FocusCount--;
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogError("타겟팅하지 않은 객체가 타겟팅 해제를 시도함!", this);
        }
#endif
    }
}
