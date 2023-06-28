using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAIController : EnemyAIController
{
    private SphereScanner _scanner;
    private EntityClassifier _filter;

    public Transform[] FirePointTr;
    public Transform BodyTr;
    public Transform GunTr;


    protected override void Awake()
    {
        base.Awake();
        _scanner = new SphereScanner(transform, data.DetectRange, data.DetectTargetLayer);
        _filter = new EntityClassifier_Robot(transform, new string[2] { "Player", "Facility" });
        Scanner = new EntitySelector(_scanner, _filter);
    }

    protected override void InitState()
    {
        AddState(new EnemyState_RobotMove(this));
        AddState(new EnemyState_RobotAttack(this));
        AddState(new EnemyState_RobotWait(this));
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDied()
    {
        base.OnDied();
    }

    public override void Attack()
    {
        BulletData bullet = data.Bullet;
        EntityManager.Instance.CreateBullet(bullet, FirePointTr[0]);
        EntityManager.Instance.CreateBullet(bullet, FirePointTr[1]);
    }

    private void LateUpdate()
    {
        if (FocusTarget)
        {
            Vector3 angle;

            Quaternion rotation = Quaternion.LookRotation(FocusTarget.position - BodyTr.position);
            angle = Vector3.zero;
            angle.y = rotation.eulerAngles.y - 90;
            BodyTr.eulerAngles = angle;

            rotation = Quaternion.LookRotation(FocusTarget.position - GunTr.position);
            angle = rotation.eulerAngles;
            angle = Vector3.zero;
            angle.z = -rotation.eulerAngles.x;
            GunTr.localEulerAngles = angle;

            //GunTr.LookAt(FocusTarget);
            //angle = Vector3.zero;
            //angle.z = -GunTr.eulerAngles.x;
            //GunTr.localEulerAngles = angle;
        }
        else
        {
            BodyTr.localEulerAngles = Vector3.zero;
            GunTr.localEulerAngles = Vector3.zero; 
        }
    }
}
