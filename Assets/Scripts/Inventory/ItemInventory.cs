using BT;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory
{
    public int Id { get; private set; }

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

    public ItemInventorySlot FindItemSlot(InventoryItemType itemType)
    {
        // ���� ��ü �˻��� ������ Ÿ���� ������Ʈ ������ ��ȯ
        for (int i = 0; i < Count; i++)
        {
            if (_inventory[i].Item.ItemType == itemType)
            {
                return _inventory[i];
            }
        }
        return null;
    }

    public bool TryFindItemSlot(InventoryItemType itemType, out ItemInventorySlot slot)
    {
        slot = null;
        for (int i = 0; i < Count; i++)
        {
            if (_inventory[i].IsEmpty == false && _inventory[i].Item.ItemType == itemType)
            {
                slot = _inventory[i];
                return true;
            }
        }
        return false;
    }

    public bool AddItem(IItem item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (_inventory[i].IsEmpty)
            {
                _inventory[i].SetItem(item);
                return true;
            }
        }
        return false;
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
            if (_inventory[i].IsEmpty)
            {
                return _inventory[i];
            }
        }
        return null;
    }

    public void SwapSlotPosition(int slotIdx1, int slotIdx2)
    {
        ItemInventorySlot slot1 = _inventory[slotIdx1];
        ItemInventorySlot slot2 = _inventory[slotIdx2];

        var tempItem = slot1.Item;

        slot1.SetItem(slot2.Item);
        slot2.SetItem(tempItem);
    }
}
