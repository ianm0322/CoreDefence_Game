using System;

[Serializable]
public class FacilityData : EntityData
{
    public BulletData Bullet;

    public FacilityData() { }
    public FacilityData(FacilityData data)
    {
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