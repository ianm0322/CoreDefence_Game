using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T:MonoBehaviour
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
        if(_instance == null)
        {
            _instance = this.GetComponent<T>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
