public abstract class EnemyDataBuilder
{
    EnemyData _data;

    public EnemyDataBuilder()
    {
        _data = new EnemyData();
    }

    public EnemyDataBuilder(EnemyData data)
    {
        _data = new EnemyData(data);
    }

    public virtual EnemyData Build()
    {
        EnemyData data = new EnemyData(_data);
        return data;
    }

    public virtual EnemyDataBuilder SetAttackTargetRange(float range)
    {
        _data.AttackTargetRange = range;
        return this;
    }
    public virtual EnemyDataBuilder SetTargetMissingRange(float range)
    {
        _data.TargetMissingRange = range;
        return this;
    }

    public virtual EnemyDataBuilder SetDetectRange(float range)
    {
        _data.DetectRange = range;
        return this;
    }
    public virtual EnemyDataBuilder SetLoseTargetDelay(float range)
    {
        _data.TargetMissingDelay = range;
        return this;
    }
    public virtual EnemyDataBuilder SetMoveSpeed(float speed)
    {
        _data.MoveSpeed = speed;
        return this;
    }
    public virtual EnemyDataBuilder SetAttackSpeedMultiple(float multiple)
    {
        _data.AttackSpeedMultiple = multiple;
        return this;
    }
    public virtual EnemyDataBuilder SetAttackDelay(float delay)
    {
        _data.AttackDelay = delay;
        return this;
    }
    public virtual EnemyDataBuilder SetAttackSpeed(float speed)
    {
        _data.AttackSpeed = speed;
        return this;
    }
    public virtual EnemyDataBuilder SetAttackDamage(int damage)
    {
        _data.AttackDamage = damage;
        return this;
    }
    public virtual EnemyDataBuilder SetAttackRange(float range)
    {
        _data.AttackRange = range;
        return this;
    }
    public virtual EnemyDataBuilder SetBulletSpeed(float speed)
    {
        _data.BulletSpeed = speed;
        return this;
    }
    public virtual EnemyDataBuilder SetBullet(BulletData bullet)
    {
        _data.Bullet = bullet;
        return this;
    }
}