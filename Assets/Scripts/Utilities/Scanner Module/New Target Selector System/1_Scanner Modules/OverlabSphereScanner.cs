using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlabSphereScanner : IScanner
{
    private Transform origin;
    private float range;
    private LayerMask layer;

    private Collider[] _cols = new Collider[8];

    public OverlabSphereScanner(Transform origin, float range, LayerMask layer)
    {
        this.origin = origin;
        this.range = range;
        this.layer = layer;
    }

    public Collider[] FindColliders()
    {
        return Scan();
    }

    private Collider[] Scan()
    {
        Physics.OverlapSphereNonAlloc(origin.position, range, _cols, layer);
        return _cols;
    }


}
