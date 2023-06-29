using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentStopNode : ExecutionNode
{
    NavMeshAgent _agent;
    Transform _tr;

    public AgentStopNode(NavMeshAgent agent)
    {
        this._agent = agent;
        this._tr = agent.transform;
    }

    protected override BTState OnUpdate()
    {
        _agent.SetDestination(_tr.position);
        return BTState.Success;
    }
}
