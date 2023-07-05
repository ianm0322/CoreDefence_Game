using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rito.CustomAttributes;

public abstract class AIController : BehaviorTree
{
    [HideInInspector]
    public CD_GameObject Body;
    [HideInInspector]
    public Rigidbody Rigid;
    [HideInInspector]
    public NavMeshAgent Agent;
    [HideInInspector]
    public Collider Collider;
    [HideInInspector]
    public Animator Anim;

    [SerializeField]
    protected Transform GroundRayTr;

    public bool IsParalysis = false;

    private Coroutine _forceCoroutine;

    protected virtual void Awake()
    {
        TryGetComponent(out Body);
        TryGetComponent(out Rigid);
        TryGetComponent(out Agent);
        TryGetComponent(out Collider);
        TryGetComponent(out Anim);
    }

    public void Impact(System.Action forceMethod)
    {
        if (_forceCoroutine != null)
            StopCoroutine(_forceCoroutine);
        _forceCoroutine = StartCoroutine(AddForceCoroutine(forceMethod));
    }

    private IEnumerator AddForceCoroutine(System.Action forceMethod)
    {
        yield return null;
        SetParalysis(true);

        forceMethod();

        RaycastHit hit;
        float t = 0f;
        while(t < 3f)
        {
            yield return null;
            if(Rigid.velocity.sqrMagnitude < 0.1f)    // 레이가 애매하게 끼어서 작동하지 않을 때를 대비한, 오브젝트가 움직이지 않는 채로 3초 이상 있으면 원래 상태로 복구하는 기능.
                t += Time.deltaTime;
            if (Physics.Raycast(GroundRayTr.position, Vector3.down, out hit, 0.1f, LayerMask.GetMask("Wall")))
            {
                break;
            }
            Debug.Log(t);
        }

        SetParalysis(false);
        Rigid.velocity = Vector3.zero;

        _forceCoroutine = null;
    }

    public void SetParalysis(bool boolean)
    {
        IsParalysis = boolean;
        if (Agent != null)
            Agent.enabled = !boolean;
        Rigid.isKinematic = !boolean;
        Collider.material.dynamicFriction = (boolean == true) ? 1f : 0f;
    }
}
