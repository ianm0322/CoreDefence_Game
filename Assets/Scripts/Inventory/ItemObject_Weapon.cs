public class ItemObject_Weapon : ItemObject
{
    public override InventoryItemType type => InventoryItemType.Weapon;
    WeaponBase weapon;

    private void Awake()
    {
        TryGetComponent(out weapon);
    }

    public override void Use()
    {
        if(weapon == null)
            TryGetComponent(out weapon);

        GameManager.Instance.player.SetWeapon(weapon);
    }

    public override void OnDisabled()
    {
        GameManager.Instance.player.SetWeapon(null);
    }
}
