using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour, IPoolingObject, IFixedUpdateListener
{
    Rigidbody rb;
    Renderer render;
    TrailRenderer trail;

    public BulletData data;
    public Ray ray;
    RaycastHit[] hit;

    public float lifeTime;
    public float lifeDist;

    public bool isDied = false;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out render);
        TryGetComponent(out trail);
    }

    public void OnFixedUpdate()
    {
        if (isDied == false)
            MoveUpdate();
    }

    private void MoveUpdate()
    {
        ray.origin = rb.position;
        ray.direction = transform.forward;

        float moveDist = data.speed * Time.fixedDeltaTime;
        Vector3 moveVec = transform.forward * moveDist;
        bool willDie = (IsLivingDist(ref moveVec) && IsLivingTime()) == false;  // �� �� �ϳ��� ���ǿ� �������� ������ true ��ȯ

        hit = Physics.RaycastAll(ray, moveDist);
        if (hit.Length > 0)  // ����ĳ��Ʈ�� �̵� ��� �����Ͽ� �浹 üũ
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (IsProperTarget(hit[i].collider))
                {
                    DamageTarget(hit[i].collider);
                    MoveAndDie(hit[i].point);
                    return;
                }
            }
        }
        if (willDie)
        {
            MoveAndDie(rb.position + moveVec);
        }
        else
        {
            rb.MovePosition(rb.position + moveVec);
            CheckOutOfBoundary();
        }
    }

    /// <summary>
    /// ����� ���� ����� ����� ������Ʈ��� �������� �ְ�, �ƴ϶�� �ƹ��� �ൿ�� ���� �ʴ´�.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    private bool DamageTarget(Collider collider)
    {
        CD_GameObject obj;
        if (collider.TryGetComponent(out obj))
        {
            obj.GiveDamage(data.damage);
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
    private bool IsProperTarget(Collider collider)
    {
        return (this.CompareTag("PlayerBullet") && collider.CompareTag("Enemy")) ||
                    (this.CompareTag("EnemyBullet") && (collider.CompareTag("Player") || collider.CompareTag("Core"))) ||
                    collider.CompareTag("Wall");
    }

    /// <summary>
    ///  ���� �̵��� �Ÿ��� ���� �Ÿ����� ª�ٸ�, vector�� �̵� ������ �Ÿ���ŭ ����ϰ� �� ����� false�� ��ȯ. �̵��Ÿ��� ����ϸ� true ��ȯ.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private bool IsLivingDist(ref Vector3 vector)
    {
        var dist = vector.magnitude;
        if (dist > lifeDist)    // �̵� �Ÿ��� ������
        {
            vector = vector.normalized * lifeDist;
            lifeDist = 0;
            return false;
        }
        else // �̵� �Ÿ��� ��������
        {
            lifeDist -= dist;
            return true;
        }
    }

    /// <summary>
    /// ���� ���� �ð����� �� ���� �Ѿ��� ����ִٸ� false ��ȯ, �ƴϸ� true ��ȯ
    /// </summary>
    /// <returns></returns>
    private bool IsLivingTime()
    {
        return Time.time - lifeTime < data.lifeTime;
    }

    private void CheckOutOfBoundary()
    {
        if (this.transform.position.sqrMagnitude > 150*150)    // if magnitude < 150
        {
            DestroyBullet();
        }
    }

    public void Init(BulletData data)
    {
        this.data = data;
        this.tag = data.tag;

        lifeTime = Time.time;
        lifeDist = data.lifeDistance;
    }

    public void MoveAndDie(Vector3 move)
    {
        rb.MovePosition(move);
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        isDied = true;
        render.enabled = false;
        yield return new WaitForSeconds(2f);
        isDied = false;
        render.enabled = true;
        EntityManager.Instance.DestroyBullet(this);
    }

    public void InitForInstantiate()
    {
    }

    public void OnCreateFromPool(object dataObj)
    {
        if(dataObj is BulletData)
        {
            BulletData _data = dataObj as BulletData;
            this.data = _data;
            this.tag = _data.tag;

            lifeTime = Time.time;
            lifeDist = _data.lifeDistance;
            trail.emitting = true;
        }
        else
            throw new System.Exception($"{name} catch wrong type data for object pooling");
    }

    public void OnPushToPool()
    {
        trail.emitting = false;
    }
}
