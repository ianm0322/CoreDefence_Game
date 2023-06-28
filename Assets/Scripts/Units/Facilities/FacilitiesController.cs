using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilitiesController : StateMachine
{
    public Transform TargetTr;

    public BulletData Bullet;
    public float AttackDelay = 3f;
    public float AttackSpeed = 1f;
    public float AttackRange = 10f;

    public EntitySelector scanner;

    private void Awake()
    {
        AddState(new FacilitiesState_Idle(this));
        AddState(new FacilitiesState_Combat(this));

        scanner = new EntitySelector(new SphereScanner(this.transform, AttackRange, LayerMask.GetMask("Enemy")), new EntityClassifier_RandomSelect(this.transform, new string[] { "Enemy" }));
        (scanner.classifier as EntityClassifier_Custom).filter += ((Collider) => { return IsBlockingObjectExist(Collider.transform); });
    }

    public virtual void Attack()
    {
        var bulletObj = EntityManager.Instance.CreateBullet(Bullet, this.transform);
        bulletObj.transform.LookAt(TargetTr);
    }

    public void SetAttackDelay(float delay)
    {
        AttackDelay = delay;
    }

    /// <summary>
    /// 타겟이 공격 가능 범위에서 벗어났다면 false, 범위 내에 있다면 true 반환.
    /// </summary>
    /// <returns></returns>
    public bool IsTargetPosible()
    {
        if (TargetTr == null || IsBlockingObjectExist(TargetTr) == true)    // 타겟이 없으면 공격대상 없음 판정. 대상과 자신 사이에 장애물이 있어도 타겟 불가.
        {
            return false;
        }
        if (Vector3.Distance(this.transform.position, TargetTr.position) < AttackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 범위 내에 타겟이 있는지 탐색.
    /// </summary>
    /// <returns></returns>
    public bool SearchTarget()
    {
        TargetTr = scanner.ScanEntity()?.transform;
        if (TargetTr != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 대상과 자신 사이에 장애물이 있으면 true 반환, 없으면 false 반환
    /// </summary>
    /// <returns></returns>
    protected bool IsBlockingObjectExist(Transform target)
    {
        if (target == null)
            return false;
        if (Physics.Linecast(transform.position, target.transform.position, out RaycastHit hit, LayerMask.GetMask("Wall")) == true)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
            return false;
        }
    }
}
