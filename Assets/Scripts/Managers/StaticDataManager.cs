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
}
