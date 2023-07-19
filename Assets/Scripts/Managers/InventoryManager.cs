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

    public void AddItem(ItemObjectInfo item)
    {
        ItemInventorySlot slot;
        if(_inventory.TryFindItemSlot(item.type, out slot))
        {
            if (slot.IncreaseItemCount(item.count))
            {
                return;
            }
        }

        _inventory.AddItem(item.ConvertToItemData());
    }
}
