using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    protected override void Awake()
    {
        base.Awake();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // ==> input manager�� �̵�
    void Update()
    {
        GameControlUpdate();
        PlayerControlUpdate();
        InventoryControlUpdate();
    }

    private void GameControlUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void PlayerControlUpdate()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(StageInfoManager.Instance != null)
            {
                StageInfoManager.Instance.InteractOnGaze();
            }
        }
    }

    private void InventoryControlUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.Instance.InventoryUI.OpenInventory();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            UIManager.Instance.InventoryUI.CloseInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // ������ ������ ���
        }
    }
}
