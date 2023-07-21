using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearerFirstPriority : AbstractPriorityCalculator
{
    private Transform _selfTr;
    private float _limitDist;

    public NearerFirstPriority(Transform self, float dist)
    {
        this._selfTr = self;
        this._limitDist = dist;
    }

    protected override float GetPriority(Collider target)
    {
        return GetDistanceRatio(target.transform);
    }

    private float GetDistanceRatio(Transform tr)
    {
        float dist = (_selfTr.position - tr.position).magnitude;
        return dist / _limitDist;
    }
}
