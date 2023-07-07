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
        UpdateManager.Instance.Join(this as IUpdateListener);
    }

    public void InitForInstantiate()
    {

    }

    public void OnCreateFromPool(object dataObj)
    {
        Builded();
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

    protected virtual void Builded()
    {
    //    Ray ray = new Ray(GroundRayTr.position, Vector3.down);
    //    RaycastHit hit;
    //    int i = 0;
    //    while (Physics.Raycast(ray, out hit, 0.1f, LayerMask.GetMask("Wall")) == false)
    //    {
    //        Debug.Log("A");
    //        this.transform.position += Vector3.down * 1f;
    //        if (++i > 1000)
    //            break;
    //    }
    //    if(hit.collider != null)
    //        Debug.Log(hit.transform.name);
    }

    public void LateUpdate()
    {
        Debug.DrawRay(GroundRayTr.position, Vector3.down * 0.1f);
    }
}
