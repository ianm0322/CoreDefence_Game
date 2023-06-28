using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WavePhase_Battle : WavePhase
{
    public override WavePhaseKind Phase => WavePhaseKind.BattlePhase;

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
        for (int i = 0; i < 1; i++)
        {
            PhaseEvent e = new EnemySpawnEvent((i+1) * 2f, 0, CTType.EnemyKind.Minion);
            waveManager.AddEvent(e);
        }
    }

    public override void OnPhaseEnd(WavePhase nextPhase)
    {
        base.OnPhaseEnd(nextPhase);
        // EventManager.KillAllMonster()
    }

    public void LoadSpawnQueue()
    {

    }
}
