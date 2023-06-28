using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    protected virtual void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform.position);
        this.transform.Rotate(Vector3.up * 180f);
    }
}
