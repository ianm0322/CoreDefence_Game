﻿using UnityEngine;
using static CTType;

public class EnemySpawnEvent : PhaseEvent
{
    public int spawnerId;
    public EnemyKind kind;
    public EnemyData data;

    public EnemySpawnEvent(float startTime, int spawnerId, EnemyKind enemy, EnemyData data = null) : base(startTime)
    {
        this.spawnerId = spawnerId;
        this.kind = enemy;
        this.data = data;
    }

    public override void Execute()
    {
        // 에네미 스폰 코드
        //EntityManager.Instance.CreateEnemy(EnemyKind.Minion, data);
        GameManager.Instance.Spawn(0);
    }
}
