using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private ItemInventory _inventory;
    public ItemInventory Inventory => _inventory;

    protected override void Awake()
    {
        base.Awake();

        _inventory = new ItemInventory();
    }

    public void AddItem(ItemObject item)
    {
        _inventory.AcquireItem(item);
    }
}
