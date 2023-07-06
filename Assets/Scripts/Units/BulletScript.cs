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
                if (_ignoreColliderId.Contains(obj.collider.GetInstanceID()) == false)   // ����� �̹� ���� �Ѿ˿� �¾Ҵ����, �Ѿ��� �� ����� ������ ������.
                {
                    if (DamageTarget(obj.collider)) // ���� ����̶�� ���� Ƚ���� �˻��ϰ�, ���� ���� �����ϸ� �ı� ���� ����.
                    {
                        _ignoreColliderId.Add(obj.collider.GetInstanceID());
                        if (++_count < Data.penetraitCount)
                        {
                            continue;
                        }
                    }
                    // ���� �ε����ų� ���� Ƚ���� ���ϸ� �Ѿ� �ı�
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
