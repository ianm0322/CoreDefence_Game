using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private List<IInitializeOnLoad> _managerListOnScene = new List<IInitializeOnLoad>();

    protected override void Awake()
    {
        base.Awake();

        // 이 오브젝트와 상위 오브젝트를 모두 불괴 설정함.
        Transform tr = this.transform;
        while (tr != null)
        {
            DontDestroyOnLoad(tr.gameObject);
            tr = tr.parent;
        }
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
}
