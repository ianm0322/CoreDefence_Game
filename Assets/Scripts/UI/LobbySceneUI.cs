using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUI : MonoBehaviour
{
    public GameObject tutorial;

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
        tutorial.SetActive(true);
        StartCoroutine(TutorialCoroutine());
    }

    private IEnumerator TutorialCoroutine()
    {
        yield return null;
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        tutorial.SetActive(false);
    }
}
