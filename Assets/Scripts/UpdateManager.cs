using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoSingleton<UpdateManager>
{
    public List<GameObject> update;

    private void Update()
    {
        IUpdateListener obj;
        for (int i = 0, end = update.Count; i < end ; i++)
        {
            if(update[i].TryGetComponent(out obj))
            {
                obj.OnUpdate();
            }
        }
    }
}
