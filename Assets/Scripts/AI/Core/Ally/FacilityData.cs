using System;

[Serializable]
public class FacilityData : EntityData
{
    public FacilityKind Kind;
    public BulletData Bullet;

    public FacilityData() { }
    public FacilityData(FacilityData data) : base(data)
    {
        Kind = data.Kind;
        MaxHp = data.MaxHp;
        Hp = data.Hp;
        AttackDamage = data.AttackDamage;
        AttackDelay = data.AttackDelay;
        AttackSpeed = data.AttackSpeed;
        AttackRange = data.AttackRange;
        BulletSpeed = data.BulletSpeed;
        MoveSpeed = data.MoveSpeed;
        Bullet = data.Bullet;
    }
}