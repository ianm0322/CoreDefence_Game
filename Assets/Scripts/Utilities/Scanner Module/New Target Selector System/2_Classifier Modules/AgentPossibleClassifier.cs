using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class AgentPossibleClassifier : AbstractClassifier
{
    private readonly float _range = 1f;

    private NavMeshAgent _agent;

    private NavMeshPath _path = new NavMeshPath();

    public AgentPossibleClassifier(NavMeshAgent agent)
    {
        this._agent = agent;
    }

    protected override bool Check(Collider target)
    {
        if (target == null) MyDebug.Log("AgentPossibleClassifier.Check(Collider): Target is null");
        return IsReachable(target.transform.position);
    }

    private bool IsReachable(Vector3 goal)
    {
        // ������Ʈ�� ���� �������� �˻��ϴ� �޼���.
        // ���: nav mesh path�� �����, ������ corner�� ��ǥ ������ �Ÿ��� �������μ� ���� ������ ��ġ���� �Ǻ��Ѵ�.
        // 1. path ���
        // 2. path�� ������ �� last ���ϱ�
        // 3. last�� goal�� �����ϸ� true, �ƴϸ� false ��ȯ

        // (1) path ���
        if (_agent == null) MyDebug.Log("A");

        bool isProperPath = _agent.CalculatePath(goal, _path);
        if (isProperPath == false)
        {
            return false;
        }

        // (2) path�� ������ �� last ���ϱ�
        Vector3[] corners = _path.corners;  // ##### �� �ڵ� �Ҿ�����(Vector[]�� ���� ������.) #####
        Vector3 last = corners[corners.Length - 1];

        // (3) last�� goal�� �����ϸ� true, �ƴϸ� false ��ȯ
        if (IsCloser(last, goal))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsCloser(Vector3 p1, Vector3 p2)
    {
        return MathUtility.CompareDist(p1 - p2, _range) < 0;
    }
}
