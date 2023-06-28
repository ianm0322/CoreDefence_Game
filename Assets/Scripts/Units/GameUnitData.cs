using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnitData : MonoBehaviour
{
    // Status
    public int MaxHp;
    public float BaseSpeed;

    private int hp;

    public int GetHp()
    {
        return hp;
    }

    public void SetHp(int hp)
    {

    }
}