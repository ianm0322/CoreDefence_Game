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
        bool willDie = (IsLivingDist(ref moveVec) && IsLivingTime()) == false;  // 둘 중 하나라도 조건에 부합하지 않으면 true 반환

        hit = Physics.RaycastAll(ray, moveDist);
        if (hit.Length > 0)  // 레이캐스트로 이동 경로 조사하여 충돌 체크
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
    /// 대상이 공격 기능을 상속한 오브젝트라면 데미지를 주고, 아니라면 아무런 행동도 하지 않는다.
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
    /// 충돌체가 총알이 피격 가능한 대상인지 검사하고, 피격 가능한 대상이면 true, 아니면 false를 반환한다.
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
    ///  만약 이동할 거리가 남은 거리보다 짧다면, vector를 이동 가능한 거리만큼 축소하고 그 결과를 false로 반환. 이동거리가 충분하면 true 반환.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private bool IsLivingDist(ref Vector3 vector)
    {
        var dist = vector.magnitude;
        if (dist > lifeDist)    // 이동 거리가 끝나면
        {
            vector = vector.normalized * lifeDist;
            lifeDist = 0;
            return false;
        }
        else // 이동 거리가 남았으면
        {
            lifeDist -= dist;
            return true;
        }
    }

    /// <summary>
    /// 만약 생존 시간보다 더 오래 총알이 살아있다면 false 반환, 아니면 true 반환
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
