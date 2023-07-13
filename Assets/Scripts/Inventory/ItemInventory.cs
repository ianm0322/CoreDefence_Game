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
            if (_inventory[i].item.name == itemName)
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
    /// �� �κ��丮 ������ ��ȯ�Ѵ�. �� ������ ������ Null���� ��ȯ�ȴ�.
    /// </summary>
    /// <returns></returns>
    private ItemInventorySlot FindEmptySlot()
    {
        for (int i = 0; i < _inventory.Count; i++)
        {
            if(_inventory[i].item == null)
            {
                return _inventory[i];
            }
        }
        return null;
    }

    /// <summary>
    /// �� ���Կ� �������� �߰��Ѵ�. �� ������ ������ �������� ������, false�� ��ȯ�ȴ�.
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
    /// �������� �κ��丮�� �߰��Ѵ�. ���� �������� �̹� �κ��丮�� ������, �� �������� ������ ������Ų��. �޼����� ���� ���ΰ� bool������ ��ȯ�ȴ�.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool AcquireItem(ItemObject item, int count = 1)
    {
        var slot = FindSlot(item.name);
        if(slot == null)
        {
            // ���� �������� ������ ������ ���� �õ�. �� ������ ���� �������� �߰��� �� ������ false ��ȯ.
            if(AddItemInstance(item, count) == false)   
            {
                return false;
            }
        }
        else
        {
            // ���� �������� ������ �� �������� ���� ����.
            slot.amount += count;
        }
        // ������ ���������� �߰������� true ��ȯ.
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
}

[System.Serializable]
public class ItemInventorySlot
{
    public ItemInventory inventory { get; private set; }
    public ItemObject item;
    public int index { get; private set; }
    public int amount;

    public ItemInventorySlot(ItemInventory inventory, int index)
    {
        this.inventory = inventory;
        this.index = index;
    }

    public void SetItem(ItemObject item, int count = 1)
    {
        this.item = item;
        this.amount = count;
    }
}

[System.Serializable]
public class ItemObject
{
    public virtual InventoryItemType type { get; }
    public Sprite icon;
    public string name;
    public GameObject gameObject;

    public virtual void OnSelected() { }
    public virtual void OnCancled() { }
}

public class InventoryItem_Weapon : ItemObject
{
    public override InventoryItemType type => InventoryItemType.Weapon;
    WeaponBase weapon;

    public InventoryItem_Weapon(GameObject obj)
    {
        gameObject = obj;
        obj.TryGetComponent(out weapon);
    }

    public override void OnCancled()
    {

    }
    public override void OnSelected()
    {

    }
}