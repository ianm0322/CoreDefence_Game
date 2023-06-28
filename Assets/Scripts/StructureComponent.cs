using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StructureComponent : MonoBehaviour
{
    [Min(0.1f)]
    public float PositionUnit = 10f;
    public float PositionYUnit = 1f;
    public Vector3 Offset01;
    public MapManager map;

    public Vector3 pos;

    private void Update()
    {
        if (map != null)
        {
            this.transform.position = map.GetGridPosition(this.transform.position);
            this.transform.position += Vector3.up * (this.transform.localScale.y * 0.5f - 2f);
        }
    }
}
