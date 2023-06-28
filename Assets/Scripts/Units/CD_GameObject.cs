using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CD_GameObject : MonoBehaviour
{
    public int hp;
    public bool IsDied { get; private set; }
    public event Action OnDied;

    public int GetHp()
    {
        return hp;
    }

    public void SetHp(int hp)
    {
        if(hp > 0)
        {
            this.hp = hp;
        }
        else
        {
            this.hp = 0;
            Die();
        }
    }

    public virtual void GiveDamage(int damage)
    {
        Debug.Log($"{name} take damage ({damage}) : {GetHp()} => {GetHp()-damage}");
        SetHp(GetHp() - damage);
    }

    public virtual void Die()
    {
        IsDied = true;
        OnDied();
    }
    public virtual void Init()
    {
        IsDied = false;
    }
}
