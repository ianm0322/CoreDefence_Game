using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : DefaultBullet
{
    protected override void MovePosition()
    {
        Vector3 moveVector = this.transform.forward * Data.speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + moveVector);
        _rigid.AddForce(Physics.gravity * Data.gravity, ForceMode.Acceleration);
    }

    public override void OnCreateFromPool(object dataObj)
    {
        base.OnCreateFromPool(dataObj);

        SetPhysics(true);
    }

    protected override void OnDamageGivingBefore(CD_GameObject obj)
    {
        base.OnDamageGivingBefore(obj); 
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
