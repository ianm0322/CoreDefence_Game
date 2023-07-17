using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : BulletBase
{
    public ParticleSystem[] ExplosionParticles;

    protected override void OnFired()
    {
        base.OnFired();
    }

    protected override void MovePosition()
    {
        Vector3 moveVector = this.transform.forward * Data.speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + moveVector);
        _rigid.AddForce(Physics.gravity * Data.gravity, ForceMode.Acceleration);
    }

    protected override void OnHit(RaycastHit[] hit)
    {
        RaycastHit obj;

        for (int i = 0; i < hit.Length; i++)
        {
            obj = hit[i];
            if (IsProperTarget(obj.collider))
            {
                // 벽에 부딪혔으면
                _rigid.position = obj.point + obj.normal * _radius;
                Explosion();
                DestroyBullet();
                return;
            }
        }
    }

    private void Explosion()
    {
        PlayParticle();

        var cols = Physics.OverlapSphere(this.transform.position, Data.explosionRange, _layer);
        AIController target;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent(out target))
            {
                Vector3 v = target.transform.position - this.transform.position;
                float distRate = 1f - (v.magnitude / Data.explosionRange);
                target.Body.GiveDamage(Mathf.FloorToInt(Data.damage * distRate));      // 폭심지와의 거리에 따른 보정값 계산 (폭심지와의 거리 / 폭발 범위 => 0..1)
                target.Impact((t) => t.AddForce((v.normalized + Vector3.up * 2).normalized * distRate * Data.explosionPower, ForceMode.VelocityChange));
            }
        }
    }

    protected override void OnDestroyed()
    {
        base.OnDestroyed();
    }

    public override void OnCreateFromPool(object dataObj)
    {
        base.OnCreateFromPool(dataObj);

        if (Data.gravity != 0)
            SetPhysics(true);
    }

    public override void OnPushToPool()
    {
        base.OnPushToPool();

        if (Data.gravity != 0)
            SetPhysics(false);
    }

    private void SetPhysics(bool enable)
    {
        _rigid.isKinematic = !enable;
    }

    private void PlayParticle()
    {
        for (int i = 0; i < ExplosionParticles.Length; i++)
        {
            ExplosionParticles[i].Play();
        }
    }
}
