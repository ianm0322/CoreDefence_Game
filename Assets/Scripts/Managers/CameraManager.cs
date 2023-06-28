using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField]
    private Camera _cam;
    [Header("Properties")]
    [SerializeField]
    private Vector3 _offset;

    public float MouseSensitive = 1f;

    private Transform _target;

    public void Init()
    {

    }

    private void LateUpdate()
    {
        _cam.transform.position = _target.position + _offset;
        Vector3 lookdir = _cam.transform.eulerAngles;
        lookdir.x += Input.GetAxis("Mouse X") * MouseSensitive;
    }
}

public static class SphereCoordUtility
{
    public static void GridToSphere(Vector3 vec, out float elevation, out float angle, out float range)
    {
        range = vec.magnitude;
        elevation = Mathf.Acos(vec.y / range);
        angle = Mathf.Atan2(vec.x, vec.z);
    }

    public static Vector3 SphereToGrid(float elevation, float angle, float range)
    {
        Vector3 result = Vector3.zero;
        result.x = Mathf.Sin(elevation) * Mathf.Sin(angle) * range;
        result.y = Mathf.Cos(elevation) * range;
        result.z = Mathf.Sin(elevation) * Mathf.Cos(angle) * range;
        return result;
    }
}
