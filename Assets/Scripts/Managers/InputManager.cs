using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override void InitOnSceneLoad(string sceneName)
    {
        if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            GameControlUpdate();
            PlayerControlUpdate();
            InventoryControlUpdate();
        }
        else if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.GameOverScene)
        {
            if (Input.anyKeyDown)
            {
                Application.Quit();
                //GameManager.Instance.LoadScene(Data.SceneKind.LobbyScene);
            }
        }
    }

    private void GameControlUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            //GameManager.Instance.PauseGame(true);
        }
    }

    private void PlayerControlUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (StageManager.Instance != null)
            {
                StageManager.Instance.InteractOnGaze();
            }
        }
    }

    private void InventoryControlUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UIManager.Instance == null) MyDebug.Log("UIManager is not exist");
            else if (UIManager.Instance.InventoryUI == null) MyDebug.Log("inventory ui is not exist");
            UIManager.Instance.InventoryUI.OpenInventory();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            UIManager.Instance.InventoryUI.CloseInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // 아이템 버리기 기능
        }
    }
}
