using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FacilityAI : AIController, IFacilityController, IUpdateListener, ITargetter
{
    public int id = SimpleID<FacilityAI>.Get();

    public FacilityData Data;
    public AIData AIInfo;

    public Vector3 facilityScale = Vector3.one;

    public Transform Target { get; set; }
    public EntitySelector Scanner { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Body.OnDiedEvent += OnDied;
    }

    public void InitForInstantiate()
    {

    }

    public void OnCreateFromPool(object dataObj)
    {
        var data = dataObj as FacilityData;
        if(data != null)
        {
            this.Data = data;
        }
    }

    public void OnPushToPool()
    {
    }

    public virtual void OnUpdate()
    {
        Operate();
    }

    public virtual void OnDied()
    {
        if (Target)
            Target.GetComponent<CD_GameObject>().FocusCount--;
    }
}
