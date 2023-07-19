using UnityEngine;

public interface IItem
{
    int Id { get; }
    InventoryItemType ItemType { get; }
    Sprite ItemIcon { get; }

    void UseItem();
    void CancleItem();
}
