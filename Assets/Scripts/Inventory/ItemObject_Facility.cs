using UnityEngine;

public class ItemObject_Facility : ItemObject
{
    public override InventoryItemType type => InventoryItemType.Turret;
    FacilityAI facility;

    private void Awake()
    {
        TryGetComponent(out facility);
    }

    public override void Use()
    {
        if (!facility)
            TryGetComponent(out facility);

        BuilderManager.Instance.SetPrefab(gameObject);
        BuilderManager.Instance.StartBuildMode();
    }

    public override void OnDisabled()
    {
        BuilderManager.Instance.StopBuildMode();
    }
}