using UnityEngine;

public class SphereScanner : ScannerModule<Transform>
{
    public Transform origin;
    public float range;
    public LayerMask layer;
    Collider[] _cols = new Collider[8];

    public SphereScanner(Transform origin, float range, LayerMask layer)
    {
        this.origin = origin;
        this.range = range;
        this.layer = layer;
    }

    public override Transform[] Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(origin.position, range, _cols, layer);
        Transform[] result = new Transform[count]; 
        for (int i = 0; i < count; i++)
        {
            result[i] = _cols[i].transform;
        }
        return result;
    }
}
