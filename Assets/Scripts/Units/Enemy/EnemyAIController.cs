
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 에네미 공용 인터페이스.
public interface IEnemyController : IPoolingObject
{
}

public class EnemyAIController : StateMachine, IEnemyController
{
    [field: SerializeField]
    public CTType.EnemyKind Kind { get; private set; }

    [HideInInspector]
    public Rigidbody Rigid;
    [HideInInspector]
    public NavMeshAgent Agent;
    [HideInInspector]
    public CD_GameObject Body;
    [HideInInspector]
    public Collider Collider;
    [HideInInspector]
    public Animator Anim;   // ######ANIMATION#######

    public EnemyData data;

    // data
    public Transform FocusTarget;
    public EntitySelector Scanner;

    protected virtual void Awake()
    {
        OnAwake();
    }


    public void InitForInstantiate()
    {
        OnAwake();
    }

    public void OnCreateFromPool(object dataObj)
    {
    }

    public void OnPushToPool()
    {
    }

    protected virtual void OnAwake()
    {
        TryGetComponent(out Rigid);
        TryGetComponent(out Agent);
        TryGetComponent(out Body);
        TryGetComponent(out Collider);
        TryGetComponent(out Anim);  // ######ANIMATION#######

        Body.OnDied += OnDied;

        stateDict = new Dictionary<string, BaseState>();
        InitState();
    }

    protected virtual void InitState()
    {
        AddState(new EnemyState_Death(this));
        AddState(new EnemyState_MassiveHit(this));
    }

    protected virtual void OnDied()
    {
        MoveState("Death");
    }

    public virtual void Attack()
    {

    }

#if true
    [SerializeField]
    private string __NowState__;

    protected void OnValidate()
    {
        if (!Rigid)
            TryGetComponent(out Rigid);
        if (!Agent)
            TryGetComponent(out Agent);
        if (!Body)
            TryGetComponent(out Body);
        if (!Collider)
            TryGetComponent(out Collider);
    }

    protected void OnGUI()
    {
        __NowState__ = CurrentState?.StateType;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, data.DetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, data.AttackRange);
        //if (CurrentState == null)
        //{
        //    switch (CurrentState.StateType)
        //    {
        //        case "GotoCore":
        //            break;
        //        case "Minion_Attack":
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
#endif
}
