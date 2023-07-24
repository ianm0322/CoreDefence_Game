using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Data
{
    public enum SceneKind
    {
        LobbyScene,
        PlayScene,
        GameOverScene,
        End
    }
}

public class GameManager : MonoSingleton<GameManager>
{
    private List<IInitializeOnLoad> _managerListOnScene = new List<IInitializeOnLoad>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        LoadScene(Data.SceneKind.LobbyScene);
    }

    public void OnSceneLoaded()
    {
        InitSceneManager();
        InitializeOnSceneLoad();
    }

    private void InitSceneManager()
    {
        _managerListOnScene.Clear();
        var managers = GameObject.FindGameObjectsWithTag("Manager");
        IInitializeOnLoad element;
        for (int i = 0; i < managers.Length; i++)
        {
            if (managers[i].TryGetComponent(out element))
            {
                _managerListOnScene.Add(element);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("초기화할 수 없는 매니저 오브젝트 감지. 정상적인 작동인지 확인하시오.", this);
            }
#endif
        }
    }

    private void InitializeOnSceneLoad()
    {
        for (int i = 0; i < _managerListOnScene.Count; i++)
        {
            _managerListOnScene[i].Init();
        }
    }

    public void LoadScene(Data.SceneKind scene)
    {
        switch (scene)
        {
            case Data.SceneKind.LobbyScene:
                {
                    SceneManager.LoadScene("LobbyScene");
                    SoundManager.Instance.PlayBGM(Data.BGMKind.BGM_Lobby);
                }
                break;
            case Data.SceneKind.PlayScene:
                {
                    SceneManager.LoadScene("LobbyScene");
                    SoundManager.Instance.PlayBGM(Data.BGMKind.BGM_Lobby);
                    LoadPlayScene();
                }
                break;
            case Data.SceneKind.GameOverScene:
                break;
            default:
                break;
        }
    }

    private void LoadPlayScene()
    {

        StageManager.Instance.Player.Spawn();
        StageManager.Instance.GiveStartingItemBundle();
    }
}