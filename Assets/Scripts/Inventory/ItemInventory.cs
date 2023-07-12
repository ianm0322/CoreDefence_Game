using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemInventory
{
    public int Id { get; private set; }

    private List<ItemInventorySlot> _inventory;

    public int Count { get; private set; }
    public int SelectedIndex { get; private set; }

    public ItemInventory(int count = 8)
    {
        Count = count;
        _inventory = new List<ItemInventorySlot>();
        for (int i = 0; i < count; i++)
        {
            _inventory.Add(new ItemInventorySlot());
        }
    }

    private ItemInventorySlot FindSlot(int itemId)
    {
        for (int i = 0; i < Count; i++)
        {
            if (_inventory[i].item.id == itemId)
            {
                return _inventory[i];
            }
        }
        return null;
    }

    public void IncreaseItemCount()
    {

    }
    // 7.12
}

public class ItemInventorySlot
{
    public InventoryItem item;
    public int index;
    public int amount;
}

public abstract class InventoryItem
{
    public abstract InventoryItemType type { get; }
    public int id;

    public abstract void OnSelected();
    public abstract void OnCancled();
}

public class InventoryItem_Weapon : InventoryItem
{
    public override InventoryItemType type => InventoryItemType.Weapon;

    public override void OnCancled()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSelected()
    {
        throw new System.NotImplementedException();
    }
}