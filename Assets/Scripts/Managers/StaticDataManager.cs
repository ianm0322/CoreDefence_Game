using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// 전역 읽기 전용 필드 정의 및 관리 매니저
/// </summary>
public class StaticDataManager : MonoSingleton<StaticDataManager>
{
    [SerializeField]
    [Header("마우스 감도")]
    private float _mouseSensitive = 1;
    public float MouseSensitive => _mouseSensitive;

    [SerializeField]
    [Header("플레이어 스폰지점 오브젝트 이름")]
    private string _playerSpawnPointName = "Player Spawn Point";
    public string PlayerSpawnPointName => _playerSpawnPointName;

    [SerializeField]
    public string PlaySceneName { get; private set; } = "PlayScene";
    public string LobbySceneName { get; private set; } = "LobbyScene";
    public string GameOverSceneName { get; private set; } = "GameOverScene";
}

public static class StaticData
{
    public static readonly float MouseSensitive = 1f;
    public static readonly string PlayerSpawnPointName = "Player Spawn Point";

    [SerializeField]
    public static string PlaySceneName { get; private set; } = "PlayScene";
    public static string LobbySceneName { get; private set; } = "LobbyScene";
    public static string GameOverSceneName { get; private set; } = "GameOverScene";

    public static float EnemyUpgradeRate { get; private set; } = 1.1f;  // 적 웨이브 레벨에 따른 업그레이드 수치(+10%)
}