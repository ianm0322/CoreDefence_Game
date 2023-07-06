using System;

[Serializable]
public class BulletData
{
    public string tag;
    public float speed;
    public int damage;
    public float lifeTime;
    public float lifeDistance;
    public int penetraitCount = 1;

    public BulletData() { }
    public BulletData(BulletData data)
    {
        tag = data.tag;
        speed = data.speed;
        damage = data.damage;
        lifeTime = data.lifeTime;
        lifeDistance = data.lifeDistance;
        penetraitCount = data.penetraitCount;
    }
}