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
    public float gravity = 0f;

    [UnityEngine.Header("Grenade Only")]
    public float explosionRange = 0f;
    public float explosionPower = 0f;

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
        gravity = data.gravity;
        explosionRange = data.explosionRange;
        explosionPower = data.explosionPower;
    }
}