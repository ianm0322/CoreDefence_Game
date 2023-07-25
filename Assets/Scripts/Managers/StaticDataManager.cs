using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// ���� �б� ���� �ʵ� ���� �� ���� �Ŵ���
/// </summary>
public class StaticDataManager : MonoSingleton<StaticDataManager>
{
    [SerializeField]
    [Header("���콺 ����")]
    private float _mouseSensitive = 1;
    public float MouseSensitive => _mouseSensitive;

    [SerializeField]
    [Header("�÷��̾� �������� ������Ʈ �̸�")]
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

    public static float EnemyUpgradeRate { get; private set; } = 1.1f;  // �� ���̺� ������ ���� ���׷��̵� ��ġ(+10%)
}