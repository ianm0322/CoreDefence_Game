using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : BulletBase
{
    int _count;
    List<int> _ignoreColliderId = new List<int>();

    protected override void OnFired()
    {
        Debug.Log(_prePos);
        _count = 0;
    }

    protected override void MovePosition()
    {
        Vector3 moveVector = this.transform.forward * Data.speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + moveVector);
    }

    protected override void OnHit(RaycastHit[] hit)
    {
        RaycastHit obj;

        for (int i = 0; i < hit.Length; i++)
        {
            obj = hit[i];
            if (IsProperTarget(obj.collider))
            {
                if (_ignoreColliderId.Contains(obj.collider.GetInstanceID()) == false)   // 대상이 이미 같은 총알에 맞았더라면, 총알은 그 대상을 완전히 무시함.
                {
                    if (DamageTarget(obj.collider)) // 공격 대상이라면 관통 횟수를 검사하고, 아직 관통 가능하면 파괴 절차 무시.
                    {
                        _ignoreColliderId.Add(obj.collider.GetInstanceID());
                        if (++_count < Data.penetraitCount)
                        {
                            continue;
                        }
                    }
                    // 벽에 부딪혔거나 관통 횟수가 다하면 총알 파괴
                    _rigid.position = obj.point + obj.normal * _radius;
                    DestroyBullet();
                    return;
                }
            }
        }
    }

    protected override void OnDestroyed()
    {
        base.OnDestroyed();
        _ignoreColliderId.Clear();
    }
}
