using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlabSphereScanner : IScanner
{
    private Transform origin;
    private float range;
    private LayerMask layer;

    public OverlabSphereScanner(Transform origin, float range, LayerMask layer)
    {
        this.origin = origin;
        this.range = range;
        this.layer = layer;
    }

    public int FindColliders(Collider[] cols)
    {
        return Overlab(cols);
    }

    private int Overlab(Collider[] cols)
    {
        int count = Physics.OverlapSphereNonAlloc(origin.position, range, cols, layer);
        return count;
    }
}
