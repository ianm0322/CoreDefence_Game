using System;

[Serializable]
public class BulletData
{
    public BulletKind type;
    public string tag;
    public float speed;
    public int damage;
    public float lifeTime;
    public float lifeDistance;
    public int penetraitCount = 1;

    public BulletData() { }
    public BulletData(BulletData data)
    {
        type = data.type;
        tag = data.tag;
        speed = data.speed;
        damage = data.damage;
        lifeTime = data.lifeTime;
        lifeDistance = data.lifeDistance;
        penetraitCount = data.penetraitCount;
    }
}