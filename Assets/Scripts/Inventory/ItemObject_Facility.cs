using UnityEngine;

public class ItemObject_Facility : IItem, ICountableItem, ILinkedItem
{
    public int Id { get; }
    public int Count { get; set; }
    public InventoryItemType ItemType { get; private set; }
    public Sprite ItemIcon { get; private set; }

    private GameObject _linkedPrefab;

    public ItemObject_Facility(InventoryItemType itemType, Sprite itemIcon, GameObject obj)
    {
        this.ItemType = itemType;
        this.ItemIcon = itemIcon;
        this.SetPrefab(obj);
    }

    public void CancleItem()
    {
        BuilderManager.Instance.StopBuildMode();
    }

    public void SetPrefab(GameObject prefab)
    {
        this._linkedPrefab = prefab;
    }

    public void UseItem()
    {
        BuilderManager.Instance.SetPrefab(_linkedPrefab);
        BuilderManager.Instance.StartBuildMode();
    }
}