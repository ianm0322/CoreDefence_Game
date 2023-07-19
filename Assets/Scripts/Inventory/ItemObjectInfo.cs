using UnityEngine;

[System.Serializable]
public class ItemObjectInfo : MonoBehaviour
{
    public InventoryItemType type;
    public Sprite icon;
    public int count;

    public ILinkedItem ConvertToItemData()
    {
        ILinkedItem item;

        if((type & InventoryItemType.Facility) != 0)
        {
            item = new ItemObject_Facility(type, icon, gameObject);
        }
        else if((type & InventoryItemType.Weapon) != 0)
        {
            item = new ItemObject_Weapon(type, icon, gameObject);
        }
        //else if((type& InventoryItemType.Item) != 0)
        //{
        //      업그레이드 아이템 생성 변환
        //}
        else
        {
            return null;
        }

        SetCount(item);

        return item;
    }

    private void SetCount(IItem item)
    {
        var countable = item as ICountableItem;
        if (countable != null)
            countable.Count = count;
    }
}
