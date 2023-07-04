using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class BTBlackboard
{
    BehaviorTree _bt;
    RootNode _root => _bt.Root;

    public event Func<int, bool> OnBTDestroyed;

    public BTBlackboard(BehaviorTree bt)
    {
        this._bt = bt;
    }

    #region 블랙보드 관리 메서드
    public void CreateBlackboard<T>()
    {
        BTBlackboardManager<T>.Instance.RegistInstance(_root.Id);
        OnBTDestroyed += BTBlackboardManager<T>.Instance.RemoveInstance;
    }

    public void DestroyBlackboard()
    {
        OnBTDestroyed(_root.Id);
        OnBTDestroyed = null;
    }

    public bool ClearBlackboard<T>()
    {
        return BTBlackboardManager<T>.Instance.RemoveInstance(_root.Id);
    }
    #endregion

    #region 데이터 설정 메서드
    public void SetData<T>(string key, T data)
    {
        if (BTBlackboardManager<T>.Instance.IsInstanceRegisted(_root.Id) == false)
        {
            CreateBlackboard<T>();
        }

        BTBlackboardManager<T>.Instance.SetData(_root.Id, key, data);
    }

    public T GetData<T>(string key)
    {
        return BTBlackboardManager<T>.Instance.GetData(_root.Id, key);
    }

    public bool RemoveData<T>(string key)
    {
        return BTBlackboardManager<T>.Instance.RemoveData(_root.Id, key);
    }

    public bool HasData<T>(string key)
    {
        return BTBlackboardManager<T>.Instance.HasData(_root.Id, key);
    }
    #endregion
}
