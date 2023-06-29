using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagedObjectPool<T> : IObjectPool<T>, IUpdateListener, IFixedUpdateListener, ILateUpdateListener where T : MonoBehaviour, IPoolingObject
{
    public EntityObjectPool<T> Pool;
    public List<T> ActiveList;

    public ManagedObjectPool(T origin, Transform parent, int capacity = 64)
    {
        Pool = new EntityObjectPool<T>(origin, parent, capacity);
        ActiveList = new List<T>();
    }

    #region interface implement
    public void Clear()
    {
        Pool.Clear();
    }

    public T CreateObject(object data)
    {
        T obj = Pool.CreateObject(data);
        ActiveList.Add(obj);
        return obj;
    }

    public void PushObject(T obj)
    {
        Pool.PushObject(obj);
        ActiveList.Remove(obj);
    }

    public void SetCapacity(int capacity)
    {
        Pool.SetCapacity(capacity);
    }
    #endregion interface implement
    public void OnFixedUpdate()
    {
        T target;
        for (int i = 0, end = ActiveList.Count; i < end; i++)
        {
            target = ActiveList[i];
            (target as IFixedUpdateListener)?.OnFixedUpdate();
        }
    }

    public void OnLateUpdate()
    {
        T target;
        for (int i = 0, end = ActiveList.Count; i < end; i++)
        {
            target = ActiveList[i];
            (target as ILateUpdateListener)?.OnLateUpdate();
        }
    }

    public void OnUpdate()
    {
        T target;
        for (int i = 0, end = ActiveList.Count; i < end; i++)
        {
            target = ActiveList[i];
            (target as IUpdateListener)?.OnUpdate();
        }
    }
}
