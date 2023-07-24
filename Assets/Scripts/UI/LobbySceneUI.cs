using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUI : MonoBehaviour
{
    public string PlaySceneName;

    public void PlayButton()
    {
        GameManager.Instance.LoadScene(Data.SceneKind.PlayScene);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void TutorialButton()
    {

    }
}
