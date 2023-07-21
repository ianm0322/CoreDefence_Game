using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class EnemyDIeNode : ExecutionNode
{
    private EnemyAI controller;

    private float _time;

    public EnemyDIeNode(EnemyAI controller)
    {
        this.controller = controller;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        _time = Time.time;

        Die();
    }

    protected override BTState OnUpdate()
    {
        if(Time.time - _time > 3f)
        {
            return BTState.Success;
        }
        else
        {
            return BTState.Running;
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        controller.Anim.SetBool("IsDied", false);
        controller.Agent.enabled = true;
        controller.Collider.enabled = true;
        EntityManager.Instance.DestroyEnemy(controller);
    }

    private void Die()
    {
        // 컴포넌트 초기화
        controller.Anim.SetBool("IsDied", true);
        controller.Agent.enabled = false;
        controller.Collider.enabled = false;

        // 타겟중이면 타겟 해제
        if (controller.Target)
            controller.Target.GetComponent<CD_GameObject>().ReleaseFocus();

        InventoryManager.Instance.AddMoney(10);
    }
}
