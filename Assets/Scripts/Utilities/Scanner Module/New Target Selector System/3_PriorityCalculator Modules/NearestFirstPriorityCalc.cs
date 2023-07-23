using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestFirstPriorityCalc : AbstractPriorityCalculator
{
    private Transform _selfTr;
    private float _limitDist;

    public NearestFirstPriorityCalc(Transform self, float dist)
    {
        this._selfTr = self;
        this._limitDist = dist;
    }

    protected override float GetPriority(Collider target)
    {
        // ����� �����̹Ƿ�, 1-(�Ÿ�/�ִ�Ÿ�)�� �Ÿ��� �������� ���� �켱���� ����
        return 1f - GetDistanceRatio(target.transform);
    }

    private float GetDistanceRatio(Transform tr)
    {
        float dist = (_selfTr.position - tr.position).magnitude;
        return dist / _limitDist;
    }
}
