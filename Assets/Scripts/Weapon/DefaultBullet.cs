using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultBullet : BulletBase
{
    int _count;
    List<int> _ignoreColliderId = new List<int>();
    Vector3 gravity;

    protected override void OnFired()
    {
        base.OnFired();
        _count = 0;

        gravity = Vector3.zero;
    }

    protected override void MovePosition()
    {
        Vector3 moveVector = this.transform.forward * Data.speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + moveVector);

        if (Data.gravity != 0)
        {
            gravity += Physics.gravity * Time.fixedDeltaTime;
            _rigid.MovePosition(_rigid.position + gravity * Time.fixedDeltaTime);
        }
        //_rigid.AddForce(Physics.gravity * Data.gravity, ForceMode.Acceleration);
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

    public override void OnCreateFromPool(object dataObj)
    {
        base.OnCreateFromPool(dataObj);

        if (Data.gravity != 0)
            SetPhysics(true);
    }

    public override void OnPushToPool()
    {
        base.OnPushToPool();

        if (Data.gravity != 0)
            SetPhysics(false);
    }

    private void SetPhysics(bool enable)
    {
        _rigid.isKinematic = !enable;
    }
}
