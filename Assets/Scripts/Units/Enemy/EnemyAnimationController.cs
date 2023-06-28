using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private EnemyAIController controller;
    private Animator anim;

    public Transform MyHeadTr;
    public Vector3 HeadOffset;

    private void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out anim);

        MyHeadTr = anim.GetBoneTransform(HumanBodyBones.Head);
        HeadOffset = new Vector3(0, 90, -90);
    }

    private void LateUpdate()
    {
        if (controller.FocusTarget)
        {
            LookAt(controller.FocusTarget.position);
        }
    }

    private void LookAt(Vector3 target)
    {
        Vector3 lookDir = (MyHeadTr.position - target).normalized;
        MyHeadTr.rotation = Quaternion.LookRotation(lookDir) * Quaternion.Euler(HeadOffset);
    }
}
