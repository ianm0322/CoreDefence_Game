using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory
{
    private WeaponBase[] _inventory;
    private bool[] _weaponActiveArray;  // 무기 활성 상태 저장 배열

    public int InventorySize { get; private set; } = 6;

    public WeaponBase this[WeaponKind index]
    {
        get
        {
            return _inventory[(int)index];
        }
    }

    public WeaponInventory(List<WeaponBase> weapons)
    {
        _inventory = new WeaponBase[InventorySize];
        _weaponActiveArray = new bool[InventorySize];
        for (int i = 0; i < _inventory.Length; i++)
        {
            _weaponActiveArray[i] = false;
            if (i < weapons.Count)
            {
                _inventory[i] = weapons[i];
            }
        }
    }

    public void SetWeaponActive(WeaponKind kind, bool enable)
    {
        _weaponActiveArray[(int)kind] = enable;
    }
}

public enum WeaponKind
{
    Pistol = 0,
    Rifle,
    Grenade,
    W3,
    W4,
    W5,
    End
}