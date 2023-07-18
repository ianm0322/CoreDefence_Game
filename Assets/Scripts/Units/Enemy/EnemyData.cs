using UnityEngine;

[System.Serializable]
public class EnemyData : EntityData
{
    [Header("AI")]
    public LayerMask DetectTargetLayer;
    /// <summary>
    /// AI�� ���� ������ �� �ִ� �Ǵ��ϴ� �Ÿ�.
    /// </summary>
    public float AttackTargetRange;  // 
    public float TargetMissingRange; // Ÿ���� �� ����� Ÿ������ �����ϴ� �Ÿ�
    public float TargetMissingDelay;  // ���� ������� �� ���� ���¸� �����ϴ� �ð�
    public float DetectRange;      // ���� ���� �����ϴ� �Ÿ�
    public float DetectDelay = 0f;

    public BulletData Bullet;

    public EnemyData() { }
    public EnemyData(EnemyData data) : base(data)
    {
        AttackTargetRange = data.AttackTargetRange;
        TargetMissingRange = data.TargetMissingRange;
        DetectRange = data.DetectRange;
        TargetMissingDelay = data.TargetMissingDelay;
        DetectDelay = data.DetectDelay;

        Bullet = new BulletData(data.Bullet);
    }
}
