using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_AttackNode : EnemyAINode
{
    private float _attackTimer;
    private bool _isAttackDone;

    private float _attackFrame = 9f / 30f;

    private bool _firstFrame;

    public Minion_AttackNode(EnemyAI controller) : base(controller)
    {
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        _attackTimer = 0f;
        _isAttackDone = false;
        _firstFrame = true;
        _controller.Anim.SetTrigger("OnAttack");
    }

    protected override BTState OnUpdate()
    {
        _attackTimer += Time.deltaTime * _controller.Data.AttackSpeed;  // �ð� ����

        if(_firstFrame) // �ִϸ����� ������Ʈ�� ���� ù ������ �����
        {
            _firstFrame = false;
            return BTState.Running;
        }

        if (IsAttackFrame())    // ���� Ÿ�̹��̸�
        {
            if (IsTargetNear()) // �Ÿ��� ����� ������ ��󿡰� ������ ����
            {
                Attack();
                _isAttackDone = true;
            }
        }
        var animInfo = _controller.Anim.GetCurrentAnimatorStateInfo(0);
        
        if (animInfo.IsName("Attack") && 
            animInfo.normalizedTime >= 0.9f)
        {
            return BTState.Success;
        }

        return BTState.Running;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    private bool IsAttackFrame()
    {
        return (_isAttackDone == false && _attackTimer > _attackFrame); 
    }

    private bool IsTargetNear()
    {
        return (_controller.GetTarget().transform.position - _controller.transform.position).sqrMagnitude 
                < _controller.Data.AttackRange * _controller.Data.AttackRange;
    }

    private void Attack()
    {
        _controller.Target.GetComponent<CD_GameObject>().GiveDamage(_controller.Data.AttackDamage);
    }
}