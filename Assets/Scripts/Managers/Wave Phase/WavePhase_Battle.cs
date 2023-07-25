using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WavePhase_Battle : WavePhase
{
    public override WavePhaseKind Phase { get; protected set; } = WavePhaseKind.BattlePhase;

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (waveManager.EventQueueIsEmpty && EntityManager.Instance.GetLiveEnemyCount() == 0)
        {
            waveManager.StartMaintenancePhase();
        }
    }

    public override void OnPhaseStart(WavePhase prePhase)
    {
        base.OnPhaseStart(prePhase);
        PushEnemySpawnPool();
    }

    public override void OnPhaseEnd(WavePhase nextPhase)
    {
        base.OnPhaseEnd(nextPhase);
        // EventManager.KillAllMonster()
    }

    // Json으로 스폰 배열을 불러오는 기능(안 만듬.)
    public void LoadSpawnQueue()
    {
        throw new System.NotImplementedException();
    }

    private void PushEnemySpawnPool()
    {
        PhaseEvent e;
        int level = waveManager.WaveLevel;
        if (level <= 0)
        {
            
        }
        else if (IsBetween(level, 1, 3))    // => 1..3 ~> minion(4+2*level)
        {
            for (int i = 0, end = 4 + 2 * level; i < end; i++)
            {
                e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.GetEnemyData(CTType.EnemyKind.Minion));
                waveManager.AddEvent(e);
            }
        }
        else if(IsBetween(level, 4, 6))     // => 4..6 ~> robot(4+2*level)
        {
            level -= 3;
            for (int i = 0, end = level + 4; i < end; i++)
            {
                e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Robot, waveManager.GetEnemyData(CTType.EnemyKind.Robot));
                waveManager.AddEvent(e);
            }
        }
        else if(IsBetween(level, 7, 10))    // => 7..10 ~> minion & robot (15+5*level)
        {
            level -= 6;
            for (int i = 0, end = 15 + 5 * level; i < end; i++)
            {
                if(i % 2 == 0)
                {
                    e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.GetEnemyData(CTType.EnemyKind.Minion));
                }
                else
                {
                    e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Robot, waveManager.GetEnemyData(CTType.EnemyKind.Robot));
                }
                waveManager.AddEvent(e);
            }
        }
        else                                // => All random
        {
            for (int i = 0, end = 30 + (level%10) * 4; i < end; i++)
            {
                int r = Random.Range(0, 4);
                if(r >= 0 && r <= 2)    // 75% 확률로 미니언, 25% 확률로 로봇 소환
                {
                    e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.GetEnemyData(CTType.EnemyKind.Minion));
                }
                else
                {
                    e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Robot, waveManager.GetEnemyData(CTType.EnemyKind.Robot));
                }
                waveManager.AddEvent(e);
            }
        }
    }

    private bool IsBetween(int a, int m, int M)
    {
        // a가 m 이상, M 이하라면 true 반환. => m ≤ a ≤ M
        return a >= m && a <= M; 
    }
}

public class PhaseEventPattern_MinionOnly
{
    protected int i = 0;

    public void Reset()
    {
        i = 0;
    }
}

public enum PhaseEventPatternKind
{
    MinionOnly,
    RobotOnly,
    OneMinionOneRobot,
    Random,
    End
}