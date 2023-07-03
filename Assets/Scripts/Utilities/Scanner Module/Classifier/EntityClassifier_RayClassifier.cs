using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자신과 대상 사이에 장애물이 없는지 검별하는 Classifier.
/// </summary>
public class EntityClassifier_RayClassifier : EntityClassifier_NearSort
{
    LayerMask _obstacleLayer = LayerMask.GetMask("Wall");

    public EntityClassifier_RayClassifier(Transform origin, string[] tags) : base(origin, tags)
    {
    }

    protected override bool Filter(Transform obj)
    {
        if (IsBlockedBetween(obj)) // 가로막는 장애물이 있으면 거름.
        {
            return false;
        }

        return base.Filter(obj);
    }

    private bool IsBlockedBetween(Transform obj)
    {
        if (Physics.Linecast(origin.position, obj.position, _obstacleLayer)) // 가로막는 장애물이 있으면 거름.
        {
            Debug.DrawLine(origin.position, obj.position, Color.red);
            return true;
        }
        else
        {
            Debug.DrawLine(origin.position, obj.position, Color.green);
            return false;
        }
    }
}
