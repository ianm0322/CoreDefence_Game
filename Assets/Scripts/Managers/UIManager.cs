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
                    if (invUi.TryGetComponent(out _inventoryUI))
                    {
                        // 인벤토리 ui script를 발견했을 때

                    }
                    else
                    {
                        // 인벤토리 ui에 인벤토리 ui script가 붙어있지 않을 때
                        // 인벤토리 ui에 스크립트 붙이고 초기화
                        Debug.LogError("UIManager can't find 'InventoryUI' object.");
                    }
                }
            }
            return _inventoryUI;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        //Init();

        MyDebug.Log("UIManager is initialized!");
    }

    public override void InitOnSceneLoad(string sceneName)
    {
        if (sceneName == StaticData.PlaySceneName)
        {
            Init();

            MyDebug.Log("UIManager is initialized!");
        }
    }

    public void Init()
    {
        FindInventoryUI();
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
                    Debug.LogError("UIManager can't find 'InventoryUI' object.");
                }
            }
#if UNITY_EDITOR
            else
            {
                // 인벤토리 ui script를 발견하지 못했을 때
                // 그냥 오류 반환하자
                Debug.LogError("UIManager can't find 'InventoryUI' object.");
            }
#endif
        }
    }
}
