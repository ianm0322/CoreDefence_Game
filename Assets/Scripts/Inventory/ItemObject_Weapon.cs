using System;
using UnityEngine;

public class ItemObject_Weapon : IItem, ILinkedItem
{
    public int Id { get; private set; }
    public InventoryItemType ItemType { get; private set; }
    public Sprite ItemIcon { get; private set; }

    WeaponBase _weapon;

    public event Action<IItem> ItemDestroyedEvent;

    public ItemObject_Weapon(InventoryItemType itemType, Sprite itemIcon, GameObject obj)
    {
        ItemType = itemType;
        ItemIcon = itemIcon;
        this.SetPrefab(obj);
    }

    public void SetPrefab(GameObject obj)
    {

        obj.TryGetComponent(out this._weapon);
    }

    public void UseItem()
    {
        StageManager.Instance.Player.SetWeapon(_weapon);
    }

    public void CancleItem()
    {
        StageManager.Instance.Player.SetWeapon(null);
    }
}