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
    /// Ÿ���� ���� ���� �������� ����ٸ� false, ���� ���� �ִٸ� true ��ȯ.
    /// </summary>
    /// <returns></returns>
    public bool IsTargetPosible()
    {
        if (TargetTr == null || IsBlockingObjectExist(TargetTr) == true)    // Ÿ���� ������ ���ݴ�� ���� ����. ���� �ڽ� ���̿� ��ֹ��� �־ Ÿ�� �Ұ�.
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
    /// ���� ���� Ÿ���� �ִ��� Ž��.
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
    /// ���� �ڽ� ���̿� ��ֹ��� ������ true ��ȯ, ������ false ��ȯ
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
