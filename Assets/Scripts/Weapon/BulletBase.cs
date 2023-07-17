using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour, IPoolingObject, IFixedUpdateListener
{
    protected Rigidbody _rigid;
    protected Renderer _render;
    protected TrailRenderer _trail;
    protected SphereCollider _collider;

    public BulletData Data;

    protected float _lifeTime;
    protected float _lifeDist;

    protected bool _isDied = false;

    protected LayerMask _layer;

    // Temp
    protected RaycastHit[] _hit;
    protected Vector3 _prePos;
    protected Vector3 _curPos;
    protected Vector3 _deltaPos;
    protected float _radius;

    private bool _onFire = false;

    protected virtual void Awake()
    {
        TryGetComponent(out _rigid);
        TryGetComponent(out _render);
        TryGetComponent(out _trail);
        TryGetComponent(out _collider);
    }

    public void OnFixedUpdate()
    {
        if (_onFire == false)    // �Ѿ� ���� �� ���� 1ȸ��
        {
            _onFire = true;
            _prePos = transform.position;
            _curPos = _prePos;
            _deltaPos = Vector3.zero;
            _trail.emitting = true;
            OnFired();
        }

        if (_isDied == false)
            MoveUpdate();
    }

    private void MoveUpdate()
    {
        CalculateDeltaPos();

        if (IsLivingTime() == false || IsLivingDist(_deltaPos) == false || CheckInsideOfBoundary() == false)
        {
            DestroyBullet();
        }

        _hit = Physics.SphereCastAll(_prePos, _radius, _deltaPos.normalized, _deltaPos.magnitude, _layer);
        if (_hit.Length > 0)
        {
            OnHit(_hit);
        }

        if (_isDied == false)
        {
            MovePosition();
        }
    }

    protected virtual void MovePosition() { }
    protected virtual void OnFired() { }
    protected abstract void OnHit(RaycastHit[] hit);
    protected virtual void OnDamageGivingBefore(CD_GameObject obj) { }
    protected virtual void OnDestroyed() { }

    #region Conditions

    /// <summary>
    /// ����� ���� ����� ����� ������Ʈ��� �������� �ְ�, �ƴ϶�� �ƹ��� �ൿ�� ���� �ʴ´�.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    protected bool DamageTarget(Collider collider)
    {
        CD_GameObject obj;
        if (collider.TryGetComponent(out obj))
        {
            OnDamageGivingBefore(obj);
            obj.GiveDamage(Data.damage);

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// �浹ü�� �Ѿ��� �ǰ� ������ ������� �˻��ϰ�, �ǰ� ������ ����̸� true, �ƴϸ� false�� ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    protected bool IsProperTarget(Collider collider)
    {
        return (
            collider.CompareTag("Wall") ||
            (this.CompareTag("PlayerBullet") && collider.CompareTag("Enemy")) ||
            (this.CompareTag("EnemyBullet") && (collider.CompareTag("Player") || collider.CompareTag("Facility") || collider.CompareTag("Core")))
            );
    }

    protected virtual void SetLayerMask()
    {
        if (CompareTag("PlayerBullet"))
        {
            _layer = LayerMask.GetMask("Wall", "Enemy");
        }
        else if (CompareTag("EnemyBullet"))
        {
            _layer = LayerMask.GetMask("Wall", "Player", "Facility");
        }
    }

    /// <summary>
    ///  ���� �̵��� �Ÿ��� ���� �Ÿ����� ª�ٸ�, vector�� �̵� ������ �Ÿ���ŭ ����ϰ� �� ����� false�� ��ȯ. �̵��Ÿ��� ����ϸ� true ��ȯ.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private bool IsLivingDist(Vector3 vector)
    {
        if (MathUtility.CompareDist(vector, _lifeDist) <= 0)
        {
            _lifeDist -= vector.magnitude;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ���� ���� �ð����� �� ���� �Ѿ��� ����ִٸ� false ��ȯ, �ƴϸ� true ��ȯ
    /// </summary>
    /// <returns></returns>
    private bool IsLivingTime()
    {
        return Time.time - _lifeTime < Data.lifeTime;
    }
    private bool CheckInsideOfBoundary()
    {
        if (MathUtility.CompareDist(this.transform.position, 500) < 0)    // if magnitude < 150
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion Conditions



    public void DestroyBullet()
    {
        OnDestroyed();
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        _isDied = true;
        _trail.emitting = false;
        yield return null;
        _render.enabled = false;
        yield return new WaitForSeconds(2f);
        _isDied = false;
        _render.enabled = true;
        EntityManager.Instance.DestroyBullet(this);
    }

    #region ���ҵ� �޼���
    // ���� ��ġ�� ���� ��ġ�� ���� �� �� ���̸� ���.
    private void CalculateDeltaPos()
    {
        _prePos = _curPos;
        _curPos = _rigid.position;
        _deltaPos = _curPos - _prePos;
    }

    #endregion

    #region Object pool interface
    public virtual void InitForInstantiate()
    {
    }

    public virtual void OnCreateFromPool(object dataObj)
    {
        if (dataObj is BulletData)
        {
            BulletData _data = dataObj as BulletData;
            this.Data = _data;
            this.tag = _data.tag;

            _lifeTime = Time.time;
            _lifeDist = _data.lifeDistance;
            _onFire = false;

            _radius = transform.localScale.x * _collider.radius;

            SetLayerMask();
        }
        else
            throw new System.Exception($"{name} catch wrong type data for object pooling");
    }

    public virtual void OnPushToPool()
    {
    }
    #endregion
}
