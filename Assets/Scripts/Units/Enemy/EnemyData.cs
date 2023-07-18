using UnityEngine;

[System.Serializable]
public class EnemyData : EntityData
{
    [Header("AI")]
    public LayerMask DetectTargetLayer;
    /// <summary>
    /// AI가 적을 공격할 수 있다 판단하는 거리.
    /// </summary>
    public float AttackTargetRange;  // 
    public float TargetMissingRange; // 타겟이 된 대상의 타겟팅을 해제하는 거리
    public float TargetMissingDelay;  // 적이 사라졌을 때 적대 상태를 유지하는 시간
    public float DetectRange;      // 적을 최초 감지하는 거리
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
