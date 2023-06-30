using UnityEngine;

public class WavePhase_Maintenance : WavePhase
{
    public override WavePhaseKind Phase { get; protected set; } = WavePhaseKind.MaintenancePhase;

    public override void OnUpdate()
    {
        base.OnUpdate();

        TimeOverUpdate();
    }

    public override void OnPhaseStart(WavePhase prePhase)
    {
        base.OnPhaseStart(prePhase);
        waveManager.WaveLevel++;
    }

    public override void OnPhaseEnd(WavePhase nextPhase)
    {
        base.OnPhaseEnd(nextPhase);
    }

    protected void TimeOverUpdate()
    {
        if (IsTimeOver())
        {
            WaveManager.Instance.SetPhase(WavePhaseKind.BattlePhase);   //전투 웨이브 실행
        }
    }

    protected bool IsTimeOver()
    {
        return ElapsedTime >= waveManager.MaintenanceTimeLimit;
    }
}
