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

public static class MyDebug
{
    public static void Log(string str)
    {
#if UNITY_EDITOR
        Debug.Log(str);
#endif
    }
}

public class GameManager : MonoSingleton<GameManager>
{
    private Data.SceneKind _currentScene;
    private bool _isPause;

    private void Start()
    {
#if UNITY_EDITOR
        LoadScene(Data.SceneKind.LobbyScene);
        MyDebug.Log("GameManager is initialized!");
#endif
    }

    public void LoadScene(Data.SceneKind scene)
    {
        _currentScene = scene;

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
                    LoadPlayScene();
                }
                break;
            case Data.SceneKind.GameOverScene:
                {
                }
                break;
            default:
                break;
        }
    }

    private void LoadPlayScene()
    {
        SoundManager.Instance.PlayBGM(Data.BGMKind.BGM_Lobby);
        SceneManager.LoadScene("PlayScene");
    }

    [System.Obsolete()]
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public Data.SceneKind GetCurrentScene()
    {
        return _currentScene;
    }

    public void PauseGame(bool isPause)
    {
        if(GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            UIManager.Instance.SetActivePausePanel(isPause);
            Time.timeScale = isPause ? 0f : 1f;
            _isPause = isPause;
        }
        else
        {
            UIManager.Instance.SetActivePausePanel(false);
            Time.timeScale = 1f;
            _isPause = false;
        }
    }

    public void GameOver()
    {
        throw new System.NotImplementedException();
    }
}