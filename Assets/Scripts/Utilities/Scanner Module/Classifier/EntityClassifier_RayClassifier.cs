using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڽŰ� ��� ���̿� ��ֹ��� ������ �˺��ϴ� Classifier.
/// </summary>
public class EntityClassifier_RayClassifier : EntityClassifier_NearSort
{
    LayerMask _obstacleLayer = LayerMask.GetMask("Wall");

    public EntityClassifier_RayClassifier(Transform origin, string[] tags) : base(origin, tags)
    {
    }

    protected override bool Filter(Transform obj)
    {
        if (IsBlockedBetween(obj)) // ���θ��� ��ֹ��� ������ �Ÿ�.
        {
            return false;
        }

        return base.Filter(obj);
    }

    private bool IsBlockedBetween(Transform obj)
    {
        if (Physics.Linecast(origin.position, obj.position, _obstacleLayer)) // ���θ��� ��ֹ��� ������ �Ÿ�.
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