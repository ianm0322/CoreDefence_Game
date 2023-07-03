using BT;
using System.Collections;
using System.Collections.Generic;

public abstract class FacilityAI : AIController, IFacilityController, IUpdateListener
{
    public int id = SimpleID<FacilityAI>.Get();

    public FacilityData Data;

    public void InitForInstantiate()
    {

    }

    public void OnCreateFromPool(object dataObj)
    {
    }

    public void OnPushToPool()
    {
    }

    public void OnUpdate()
    {
        Operate();
    }

    protected virtual void Update()
    {
        OnUpdate();
    }
}
