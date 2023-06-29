using UnityEngine;
using static CTType;

public class EnemySpawnEvent : PhaseEvent
{
    public int spawnerId;
    public EnemyKind kind;
    public EnemyData data;

    public EnemySpawnEvent(float startTime, int spawnerId, EnemyKind enemy, EnemyData data) : base(startTime)
    {
        this.spawnerId = spawnerId;
        this.kind = enemy;
        this.data = data;
    }

    public override void Execute()
    {
        // 에네미 스폰 코드
        var enemy = EntityManager.Instance.CreateEnemy(EnemyKind.Minion, data);
        enemy.Agent.Warp(GameManager.Instance.Spawners[0].transform.position);
        //GameManager.Instance.Spawn(0);
    }
}
