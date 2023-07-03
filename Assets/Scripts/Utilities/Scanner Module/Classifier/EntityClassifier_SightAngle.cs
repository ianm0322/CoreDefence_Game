using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 시야각 내의 대상을 검별하는 Classifier.
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
        Vector3 v1 = origin.forward;                                    // 본체의 정면 벡터
        Vector3 v2 = (obj.position - origin.position).normalized;       // 본체에서 타겟으로 향하는 노말 벡터
        float cosined_theta = Vector3.Dot(v1, v2);  // 정면에 가까울수록 1, 반대에 가까워질수록 -1이 되는 값 cosined_theta.



        if (cosined_theta > Mathf.Cos(SightAngle * Mathf.Deg2Rad))    // 본테 정면과 타겟 벡터 사잇각이 시야각 안에 있다면
            return false;

        return base.Filter(obj);
    }
}
