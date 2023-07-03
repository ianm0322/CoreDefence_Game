using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �þ߰� ���� ����� �˺��ϴ� Classifier.
/// </summary>
public class EntityClassifier_SightAngle : EntityClassifier_RayClassifier
{
    public float SightAngle { get; set; }

    public EntityClassifier_SightAngle(Transform origin, float sightAngle, string[] tags) : base(origin, tags)
    {
        this.SightAngle = sightAngle;
    }

    protected override bool Filter(Transform obj)
    {
        Vector3 v1 = origin.forward;                                    // ��ü�� ���� ����
        Vector3 v2 = (obj.position - origin.position).normalized;       // ��ü���� Ÿ������ ���ϴ� �븻 ����
        float cosined_theta = Vector3.Dot(v1, v2);  // ���鿡 �������� 1, �ݴ뿡 ����������� -1�� �Ǵ� �� cosined_theta.



        if (cosined_theta > Mathf.Cos(SightAngle * Mathf.Deg2Rad))    // ���� ����� Ÿ�� ���� ���հ��� �þ߰� �ȿ� �ִٸ�
            return false;

        return base.Filter(obj);
    }
}
