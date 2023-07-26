using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                        MyDebug.Log("UIManager can't find 'InventoryUI' object.");
                    }
                }
            }
            return _inventoryUI;
        }
    }

    [SerializeField]
    GameObject _pausePanel;

    [SerializeField]
    Slider _reloadSlider;


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

        GameObject.Find(StaticData.UIReloadSlider).TryGetComponent(out _reloadSlider);
        if (_reloadSlider == null)
            MyDebug.Log($"UIManager couldn't find \"{StaticData.UIReloadSlider}\"");
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
                    MyDebug.Log("UIManager can't find 'InventoryUI' object.");
                }
            }
#if UNITY_EDITOR
            else
            {
                // �κ��丮 ui script�� �߰����� ������ ��
                // �׳� ���� ��ȯ����
                MyDebug.Log("UIManager can't find 'InventoryUI' object.");
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

    public void SetReloadSliderPrograss(float prograss)
    {
        if (_reloadSlider != null)
        {
            prograss = Mathf.Clamp01(prograss);
            if (prograss == 0f)
            {
                _reloadSlider.enabled = false;
            }
            else
            {
                _reloadSlider.enabled = true;
                _reloadSlider.value = prograss;
            }
        }
    }

    public void ResetReloadSliderPrograss()
    {
        if (_reloadSlider != null)
        {
            _reloadSlider.value = 0f;
            _reloadSlider.enabled = false;
        }
    }
}
