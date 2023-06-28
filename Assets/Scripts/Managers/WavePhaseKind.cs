using System;

[Serializable]
public enum WavePhaseKind
{
    None = -1,
    MaintenancePhase,
    BattlePhase,
    GameOver,
    End
}