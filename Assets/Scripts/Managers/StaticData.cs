using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class StaticData
{
    public static readonly float MouseSensitive = 1f;
    public static readonly float RespawnDelay = 3f;
    public static readonly string PlayerSpawnPointName = "Player Spawn Point";

    [SerializeField]
    public static string PlaySceneName { get; private set; } = "PlayScene";
    public static string LobbySceneName { get; private set; } = "LobbyScene";
    public static string GameOverSceneName { get; private set; } = "GameOverScene";

    public static float EnemyUpgradeRate { get; private set; } = 1.1f;  // 적 웨이브 레벨에 따른 업그레이드 수치(+10%)

    public static string UIPausePanelName { get; private set; } = "PausePanel";
    public static string UIReloadSlider { get; private set; } = "ReloadBar";

    public static int Layer_Player { get; private set; } = 7;
    public static int Layer_Ghost { get; private set; } = 11;
}