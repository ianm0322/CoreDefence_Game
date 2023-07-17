using UnityEngine;

[System.Serializable]
public class ItemObject : MonoBehaviour
{
    public virtual InventoryItemType type { get; }
    public Sprite icon;

    public virtual void Use() { }
    public virtual void OnDisabled() { }
}
