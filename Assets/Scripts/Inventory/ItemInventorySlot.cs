[System.Serializable]
public class ItemInventorySlot
{
    private ItemInventory _parent { get; set; }
    public int Index { get; private set; }
    public IItem Item { get; private set; }

    public bool IsEmpty => Item == null;


    public ItemInventorySlot(ItemInventory inventory, int index)
    {
        this._parent = inventory;
        this.Index = index;
    }

    public void SetItem(IItem item)
    {
        this.Item = item;
    }
    public bool UseItem()
    {
        if (!IsEmpty)
        {
            Item.UseItem();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem()
    {
        this.Item = null;
    }

    public bool IncreaseItemCount(int count)
    {
        var countable = Item as ICountableItem;
        if (countable != null)
        {
            countable.Count += count;
        }
        return countable != null;   // 아이템 추가 성공 여부 반환
    }

    public bool DecreaseItemCount(int count)
    {
        var countable = Item as ICountableItem;
        if (countable != null)
        {
            countable.Count -= count;
            if (countable.Count <= 0)
                RemoveItem();
        }
        return countable != null;   // 아이템 감소 성공 여부 반환
    }

    /// <summary>
    /// 아이템을 갯수만큼 제거. 단, 보유한 아이템이 제거량보다 부족하면 제거하지 않고 false 반환.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool TryDecreaseItemCount(int count)
    {
        var countable = Item as ICountableItem;
        if (countable != null)
        {
            // 아이템이 충분하면 정상 작동하고 true 반환
            if(countable.Count >= count)
            {
                DecreaseItemCount(count);
                return true;
            }
        }

        // 아이템이 없거나 부족하면 false 반환.
        return false;
    }

    public int GetCount()
    {
        if (Item == null)
        {
            return 0;   // 아이템 is null => 0개
        }
        else
        {
            var countable = Item as ICountableItem;
            // 카운터블 아이템이면 개수 반환
            if(countable != null)
            {
                return countable.Count;
            }
            // 논 카운터블이면 1개만 있음(==고유함) 반환
            else
            {
                return 1;
            }
        }
    }
}