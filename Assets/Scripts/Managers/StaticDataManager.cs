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
}
