using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemInventory
{
    List<ItemInventorySlot> _inventory;

    //public void AddItem(int itemId, int count)
    //{

    //}

    //public void RemoveItem(int itemId, int count)
    //{

    //}

    //public InventoryItem GetItem(int index)
    //{

    //}

    //public int GetItemAmount(int itemId)
    //{

    //}
}

public class ItemInventorySlot
{
    public InventoryItem item;
    public int index;
    public int amount;
}

public class InventoryItem
{
    public InventoryItemType type;
    public int id;
}