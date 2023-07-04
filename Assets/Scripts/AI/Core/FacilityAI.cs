using BT;
using System.Collections;
using System.Collections.Generic;

public abstract class FacilityAI : AIController, IFacilityController, IUpdateListener
{
    public int id = SimpleID<FacilityAI>.Get();

    public FacilityData FacilityInfo;
    public AIData AIInfo;

    public void InitForInstantiate()
    {

    }

    public void OnCreateFromPool(object dataObj)
    {
    }

    public void OnPushToPool()
    {
    }

    public virtual void OnUpdate()
    {
        Operate();
    }
}
