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
        // 가까운 순서이므로, 1-(거리/최대거리)로 거리가 가까울수록 높은 우선순위 배정
        return 1f - GetDistanceRatio(target.transform);
    }

    private float GetDistanceRatio(Transform tr)
    {
        float dist = (_selfTr.position - tr.position).magnitude;
        return dist / _limitDist;
    }
}
