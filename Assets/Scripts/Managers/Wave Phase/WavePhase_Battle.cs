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
            waveManager.SetPhase(WavePhaseKind.MaintenancePhase);
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

    public void LoadSpawnQueue()
    {

    }

    public virtual void PushEnemySpawnPool()
    {
        PhaseEvent e;
        switch (waveManager.WaveLevel)
        {
            case 1:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        //e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.TEMP_MINION_DATA);   // null 말고 다른 데이터값
                        e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Robot, waveManager.TEMP_ROBOT_DATA);   // null 말고 다른 데이터값
                        waveManager.AddEvent(e);
                    }
                }
                break;
            case 2:
                {
                    for (int i = 0; i < 5; i++)
                    {
                        e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.TEMP_MINION_DATA);   // null 말고 다른 데이터값
                        waveManager.AddEvent(e);
                    }
                }
                break;
            case 3:
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if ((i & 1) == 0)
                        {
                            e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.TEMP_MINION_DATA);   // null 말고 다른 데이터값
                        }
                        else
                        {
                            e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Robot, waveManager.TEMP_ROBOT_DATA);   // null 말고 다른 데이터값
                        }
                        waveManager.AddEvent(e);
                    }
                }
                break;
            case 4:
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if ((i % 4) == 0)
                        {
                            e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Robot, waveManager.TEMP_ROBOT_DATA);   // null 말고 다른 데이터값
                        }
                        else
                        {
                            e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.TEMP_MINION_DATA);   // null 말고 다른 데이터값
                        }
                        waveManager.AddEvent(e);
                    }
                }
                break;
            case 99:
                {
                    e = new EnemySpawnEvent(0, 0, CTType.EnemyKind.Minion, waveManager.TEMP_MINION_DATA);
                    waveManager.AddEvent(e);
                }
                break;
            default:
                {
                    for (int i = 0; i < waveManager.WaveLevel * 2; i++)
                    {
                        int rand = Random.Range(0, 9);
                        if (rand < 3)
                        {
                            e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Robot, waveManager.TEMP_ROBOT_DATA);   // null 말고 다른 데이터값
                        }
                        else
                        {
                            e = new EnemySpawnEvent((i + 1) * 2f, 0, CTType.EnemyKind.Minion, waveManager.TEMP_MINION_DATA);   // null 말고 다른 데이터값
                        }
                        waveManager.AddEvent(e);
                    }
                }
                break;
        }
    }
}
