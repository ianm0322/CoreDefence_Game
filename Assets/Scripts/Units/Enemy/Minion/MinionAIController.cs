using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAIController : EnemyAIController
{
    private SphereScanner _scanner;
    private EntityClassifier_MeleeAgent _filter;

    protected override void Awake()
    {
        base.Awake();
        _scanner = new SphereScanner(transform, data.DetectRange, data.DetectTargetLayer);
        _filter = new EntityClassifier_MeleeAgent(transform, Agent, new string[2] { "Player", "Facility" });
        Scanner = new EntitySelector(_scanner, _filter);
    }

    protected override void InitState()
    {
        AddState(new EnemyState_GotoCore(this));
        AddState(new EnemyState_MinionAttackReady(this));
        AddState(new EnemyState_Death(this));
        AddState(new EnemyState_MinionAttack(this));
        AddState(new EnemyState_ChaseTarget(this));
    }

    protected override void OnDied()
    {
        base.OnDied();
    }

    public override void Attack()
    {
        CD_GameObject target;

        if(FocusTarget.TryGetComponent(out target))
        {
            target.GiveDamage(data.AttackDamage);
        }
    }
}
