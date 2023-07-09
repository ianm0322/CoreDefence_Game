using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : DefaultBullet
{
    protected override void MovePosition()
    {
        Vector3 moveVector = this.transform.forward * Data.speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + moveVector);
        _rigid.AddForce(Physics.gravity, ForceMode.Acceleration);
    }

    public override void OnCreateFromPool(object dataObj)
    {
        base.OnCreateFromPool(dataObj);

        SetPhysics(true);
    }

    public override void OnPushToPool()
    {
        base.OnPushToPool();
     
        SetPhysics(false);
    }

    private void SetPhysics(bool enable)
    {
        _rigid.isKinematic = !enable;
    }
}
