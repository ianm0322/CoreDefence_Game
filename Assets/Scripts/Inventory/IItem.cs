using System;
using UnityEngine;

public interface IItem
{
    int Id { get; }
    InventoryItemType ItemType { get; }
    Sprite ItemIcon { get; }
    event Action<IItem> ItemDestroyedEvent;

    void UseItem();
    void CancleItem();
}
