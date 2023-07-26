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
                    // 인벤토리 ui script를 발견했을 때

                }
                else
                {
                    // 인벤토리 ui에 인벤토리 ui script가 붙어있지 않을 때
                    // 인벤토리 ui에 스크립트 붙이고 초기화
                    MyDebug.Log("UIManager can't find 'InventoryUI' object.");
                }
            }
#if UNITY_EDITOR
            else
            {
                // 인벤토리 ui script를 발견하지 못했을 때
                // 그냥 오류 반환하자
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
