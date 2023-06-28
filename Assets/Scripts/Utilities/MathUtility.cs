using System;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    /// <summary>
    /// 한 점과 한 닫힌 벡터 사이의 가장 가까운 점
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Vector3 NearestPoint(Vector3 startPoint, Vector3 endPoint, Vector3 p)
    {
        Vector3 line = endPoint - startPoint;
        Vector3 originToP = p - startPoint;
        float t = (Vector3.Dot(line, originToP) / line.sqrMagnitude);  // p의 위치를 line에 투영시킨 뒤, line의 길이로 나눠, p로 분절된 line의 비율 = t 구함.
        Vector3 result = line * Mathf.Clamp01(t) + startPoint;  // line 위의 점 위치 구함
        return result;
    }

    /// <summary>
    /// 한 점과 한 열린 벡터 사이의 가장 가까운 점.
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Vector3 NearestPointUnclamped(Vector3 startPoint, Vector3 endPoint, Vector3 p)
    {
        Vector3 line = endPoint - startPoint;
        Vector3 originToP = p - startPoint;
        float t = (Vector3.Dot(line, originToP) / line.sqrMagnitude);  // p의 위치를 line에 투영시킨 뒤, line의 길이로 나눠, p로 분절된 line의 비율 = t 구함.
        Vector3 result = line * t + startPoint;  // line 위의 점 위치 구함
        return result;
    }

    /// <summary>
    /// 한 점과 한 벡터 사이의 가장 가까운 거리의 제곱값
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static float NearestPointSqrDist(Vector3 startPoint, Vector3 endPoint, Vector3 p)
    {
        Vector3 nearPoint = NearestPoint(startPoint, endPoint, p);
        return (nearPoint - p).sqrMagnitude;
    }

    /// <summary>
    /// Counter-ClockWise 함수. xz 평면상의 관계에 대한 ccw값을 반환한다.
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <returns>CCW > 0: clockwise, CCW == 0: straight, CCW < 0: counter-clockwise.</returns>
    public static float CCW(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float result = p1.z * p2.x + p2.z * p3.x + p3.z * p1.x;
        result = result - (p1.x * p2.z + p2.x * p3.z + p3.x * p1.z);
        return result;
    }

    public static int CompareDist(Vector3 v, float dist)
    {
        float magnitude = v.sqrMagnitude;
        dist = dist * dist;
        if(magnitude > dist)
        {
            return 1;
        }
        else if(magnitude < dist)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}