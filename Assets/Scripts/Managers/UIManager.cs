using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>, IInitializeOnLoad
{
    [SerializeField]
    InventoryUIScript _inventoryUI;
    public InventoryUIScript InventoryUI
    {
        get
        {
            if (_inventoryUI == null)
            {
                var invUi = GameObject.Find("InventoryUI");
                if (invUi != null)
                {
                    if (!invUi.TryGetComponent(out _inventoryUI))
                    {
                        Debug.LogError("UIManager can't find 'InventoryUI' object.");
                    }
                }
            }
            return _inventoryUI;
        }
    }

    [SerializeField]
    GameObject _pausePanel;

    protected override void Awake()
    {
        base.Awake();

        //Init();

        MyDebug.Log("UIManager is initialized!");
    }

    public override void InitOnSceneLoad(string sceneName)
    {
        if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            Init();

            MyDebug.Log("UIManager is initialized!");
        }
    }

    public void Init()
    {
        FindInventoryUI();
        _pausePanel = GameObject.Find(StaticData.UIPausePanelName);
        if (_pausePanel == null) 
            MyDebug.Log($"UIManager couldn't find \"{StaticData.UIPausePanelName}\"");
    }

    private void FindInventoryUI()
    {
        if (_inventoryUI == null)
        {
            var invUi = GameObject.Find("InventoryUI");
            if (invUi != null)
            {
                if (invUi.TryGetComponent(out _inventoryUI))
                {
                    // �κ��丮 ui script�� �߰����� ��

                }
                else
                {
                    // �κ��丮 ui�� �κ��丮 ui script�� �پ����� ���� ��
                    // �κ��丮 ui�� ��ũ��Ʈ ���̰� �ʱ�ȭ
                    Debug.LogError("UIManager can't find 'InventoryUI' object.");
                }
            }
#if UNITY_EDITOR
            else
            {
                // �κ��丮 ui script�� �߰����� ������ ��
                // �׳� ���� ��ȯ����
                Debug.LogError("UIManager can't find 'InventoryUI' object.");
            }
#endif
        }
    }

    [System.Obsolete()]
    public void SetActivePausePanel(bool enabled)
    {
        //if(_pausePanel != null)
        //    _pausePanel.SetActive(enabled);
    }
}
