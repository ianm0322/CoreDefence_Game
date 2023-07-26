using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this.GetComponent<T>();
            Transform tr = _instance.transform;
            while (tr.parent != null)
            {
                tr = tr.parent;
            }
            DontDestroyOnLoad(tr.gameObject);
        }
        else
        {
            _instance.InitOnSceneLoad(SceneManager.GetActiveScene().name);
            Destroy(this.gameObject);
        }
    }

    public virtual void InitOnSceneLoad(string sceneName)
    {
    }
}
