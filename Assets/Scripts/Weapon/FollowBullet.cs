using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : DefaultBullet
{
    Transform target;

    protected override void OnFired()
    {
        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, transform.forward, out hit, float.PositiveInfinity, _layer))
        {
            target = hit.transform;
        }
    }

    protected override void MovePosition()
    {
        transform.LookAt(target);
        _rigid.velocity = transform.forward * Data.speed;
    }

    protected override void OnHit(RaycastHit[] hit)
    {
        RaycastHit obj;

        for (int i = 0; i < hit.Length; i++)
        {

            obj = hit[i];
            if (IsProperTarget(obj.collider))
            {
                MyDebug.Log(obj.collider.name);
                DamageTarget(obj.collider);
                _rigid.position = obj.point + obj.normal * _radius;
                DestroyBullet();
                return;
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
