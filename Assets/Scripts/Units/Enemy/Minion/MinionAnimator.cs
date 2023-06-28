using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAnimator : MonoBehaviour
{
    MinionAIController controller;
    EnemyBody body;
    Animator anim;
    NavMeshAgent agent;

    private void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out body);
        TryGetComponent(out anim);
        TryGetComponent(out agent);
        
        controller.StateMachineEvent += OnStateMachineListener;
    }

    private void LateUpdate()
    {
        anim.SetFloat("MoveVelocity", agent.desiredVelocity.sqrMagnitude * 0.1f);
        anim.SetFloat("AttackSpeed", controller.data.AttackSpeed);
        anim.SetBool("IsDied", body.IsDied);
    }

    private void OnStateMachineListener(object sender, System.EventArgs e)
    {
        StateMachineEventListenerArgs args = e as StateMachineEventListenerArgs;
        if(args != null)
        {
            if (args.newState.StateType == "MinionAttack")
            {
                anim.SetTrigger("OnAttack");
            }
        }
    }
}
