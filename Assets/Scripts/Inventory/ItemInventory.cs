using BT;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInventory
{
    public int Id { get; private set; }

    [SerializeField]
    private List<ItemInventorySlot> _inventory;

    //public int Count { get; private set; }
    public int Count => _inventory.Count;
    public int SelectedIndex { get; private set; }

    public ItemInventory(int count = 8)
    {
        //Count = count;
        _inventory = new List<ItemInventorySlot>();
        ItemInventorySlot slot;
        for (int i = 0; i < count; i++)
        {
            slot = new ItemInventorySlot(this, i);
            _inventory.Add(slot);
        }
    }

    public ItemInventorySlot FindSlot(string itemName)
    {
        for (int i = 0; i < Count; i++)
        {
            // 슬롯의 아이템
            if (_inventory[i].IsEmpty == false && _inventory[i].Item.name == itemName)
            {
                return _inventory[i];
            }
        }
        return null;
    }
    public ItemInventorySlot GetSlot(int index)
    {
        return _inventory[index];
    }

    /// <summary>
    /// 빈 인벤토리 슬롯을 반환한다. 빈 슬롯이 없으면 Null값이 반환된다.
    /// </summary>
    /// <returns></returns>
    private ItemInventorySlot FindEmptySlot()
    {
        for (int i = 0; i < _inventory.Count; i++)
        {
            if(_inventory[i].IsEmpty)
            {
                return _inventory[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 빈 슬롯에 아이템을 추가한다. 빈 슬롯이 없으면 실행하지 않으며, false가 반환된다.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool AddItemInstance(ItemObject item, int count = 1)
    {
        var slot = FindEmptySlot();
        if(slot == null)
        {
            return false;
        }
        else
        {
            slot.SetItem(item, count);
            return true;
        }
    }

    /// <summary>
    /// 아이템을 인벤토리에 추가한다. 같은 아이템이 이미 인벤토리에 있으면, 그 아이템의 개수를 증가시킨다. 메서드의 성공 여부가 bool값으로 반환된다.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool AcquireItem(ItemObject item, int count = 1)
    {
        var slot = FindSlot(item.name);
        if(slot == null)
        {
            // 기존 아이템이 없으면 아이템 생성 시도. 빈 슬롯이 없어 아이템을 추가할 수 없으면 false 반환.
            if(AddItemInstance(item, count) == false)   
            {
                return false;
            }
        }
        else
        {
            // 기존 아이템이 있으면 그 아이템의 개수 증가.
            slot.amount += count;
        }
        // 아이템 정상적으로 추가됐으면 true 반환.
        return true;
    }

    public bool ConsumeItem(ItemObject item, int count)
    {
        var slot = FindSlot(item.name);
        if(slot == null)
        {
            return false;
        }
        else
        {
            slot.amount -= count;
            return true;
        }
    }

    public void SwapSlotPosition(int slotIdx1, int slotIdx2)
    {
        ItemInventorySlot slot1 = _inventory[slotIdx1];
        ItemInventorySlot slot2 = _inventory[slotIdx2];

        var tempItem = slot1.Item;
        var tempAmount = slot1.amount;

        slot1.SetItem(slot2.Item, slot2.amount);
        slot2.SetItem(tempItem, tempAmount);
    }
}

[System.Serializable]
public class ItemInventorySlot
{
    private ItemInventory _inventory { get; set; }
    public ItemObject Item { get; private set; }

    public bool IsEmpty => Item == null;

    public int index { get; private set; }
    public int amount;

    public ItemInventorySlot(ItemInventory inventory, int index)
    {
        this._inventory = inventory;
        this.index = index;
    }

    public void SetItem(ItemObject item, int count = 1)
    {
        this.Item = item;
        this.amount = count;
    }
    public bool UseItem()
    {
        if (!IsEmpty)
        {
            Item.Use();
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public class ItemObject
{
    public virtual InventoryItemType type { get; }
    public Sprite icon;
    public string name;
    public GameObject gameObject;

    public ItemObject(GameObject obj, string name, Sprite icon)
    {
        this.gameObject = obj;
        this.name = name;
        this.icon = icon;
    }

    public virtual void Use() { }
    public virtual void OnDisabled() { }
}

public class InventoryItem_Weapon : ItemObject
{
    public override InventoryItemType type => InventoryItemType.Weapon;
    WeaponBase weapon;

    public InventoryItem_Weapon(GameObject obj, string name, Sprite icon) : base(obj, name, icon)
    {
        gameObject = obj;
        obj.TryGetComponent(out weapon);
    }

    public override void OnDisabled()
    {
        weapon.gameObject.SetActive(false);
        GameManager.Instance.player._weapon = weapon;
    }

    public override void Use()
    {
        weapon.SetEquipTransform();
        GameManager.Instance.player._weapon = weapon;
        weapon.gameObject.SetActive(true);
    }
}