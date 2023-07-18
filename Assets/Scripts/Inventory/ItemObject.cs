using UnityEngine;

[System.Serializable]
public abstract class ItemObject : MonoBehaviour
{
    public virtual InventoryItemType type { get; }
    public Sprite icon;

    public abstract void Use();
    public abstract void OnDisabled();
}
