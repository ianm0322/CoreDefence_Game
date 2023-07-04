using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpdateManager : MonoSingleton<TestUpdateManager>
{
    public List<GameObject> update;

    private void Update()
    {
        foreach (var item in update)
        {
            IUpdateListener l;
            item.TryGetComponent(out l);
            l.OnUpdate();
        }
    }
}
