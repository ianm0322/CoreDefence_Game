using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> where T : class, new()
{
    private Queue<T> pool;
    
    public int Capacity { get; private set; }

    public void SetCapacity(int capacity)
    {

    }

    public T GetObject()
    {
        if(pool.Count == 0)
        {
            SetCapacity(Capacity * 2);
        }

        T obj = pool.Dequeue();
        return obj;
    }

    public void ReturnObject(T obj)
    {
        pool.Enqueue(obj);
    }
}
