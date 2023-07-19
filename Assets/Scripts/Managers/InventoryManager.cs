using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private ItemInventory _inventory;
    public ItemInventory Inventory => _inventory;

    private int _money;
    public int Money => _money;

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

    public void AddMoney(int value)
    {
        _money += value;
    }

    public void SubtractMoney(int value)
    {
        _money = _money < value ? _money - value : 0;
    }

    public bool CanSpendMoney(int price)
    {
        return Money >= price;
    }
}
