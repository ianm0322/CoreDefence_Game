using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPoolingObject
{
    Queue<T> pool;
    Queue<T> allObjects;
    public T Origin;
    public Transform Parent;
    public int Capacity { get; private set; } = 0;

    public EntityObjectPool(T origin, Transform parent, int capacity = 64)
    {
        pool = new Queue<T>();
        allObjects = new Queue<T>();

        Origin = origin;
        Parent = parent;

        SetCapacity(capacity);
    }

    /// <summary>
    /// 해당 풀의 모든 오브젝트 제거
    /// </summary>
    public void Clear()
    {
        while(allObjects.Count > 0)
        {
            T obj = allObjects.Dequeue();
            Object.Destroy(obj.gameObject);
        }
        pool.Clear();
        allObjects.Clear();
    }

    public T CreateObject(object data)
    {
        if (pool.Count == 0)
        {
            SetCapacity(Capacity * 2);
        }
        // 만들고
        T obj = pool.Dequeue(); // 풀에서 dequeue
        obj.transform.parent = null;
        obj.gameObject.SetActive(true);
        obj.OnCreateFromPool(data);  // 초기화
        return obj;
    }

    public void PushObject(T obj)
    {
        // 비활성하고
        obj.gameObject.SetActive(false);
        // 초기화하고
        obj.transform.parent = Parent;
        // 풀에 넣기
        obj.OnPushToPool();
        pool.Enqueue(obj);
    }

    public void SetCapacity(int capacity)
    {
        if(capacity > Capacity)
        {
            for (int i = Capacity; i < capacity; i++)
            {
                Instantiate();
            }
            Capacity = capacity;
        }
        else if(capacity < Capacity)
        {
            T obj;
            for (int i = capacity; i < Capacity; i++)
            {
                if(pool.TryDequeue(out obj))
                {
                    Object.Destroy(obj);
                }
                else
                {
                    break;
                }
            }
        }
    }

    private T Instantiate()
    {
        var obje = GameObject.Instantiate(Origin);
        T obj;
        obje.TryGetComponent(out obj);
        //T obj = GameObject.Instantiate(Origin);
        obj.transform.parent = Parent;
        obj.InitForInstantiate();
        obj.gameObject.SetActive(false);

        pool.Enqueue(obj);
        allObjects.Enqueue(obj);

        return obj;
    }
}

public interface IObjectPool<T> where T : MonoBehaviour, IPoolingObject
{
    void SetCapacity(int capacity);
    T CreateObject(object data);
    void PushObject(T obj);
    void Clear();
}

public interface IPoolingObject
{
    void InitForInstantiate();
    void OnCreateFromPool(object dataObj);
    void OnPushToPool();
}
