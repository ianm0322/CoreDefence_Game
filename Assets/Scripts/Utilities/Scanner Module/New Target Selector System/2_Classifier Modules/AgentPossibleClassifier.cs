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
        // 에이전트가 도달 가능한지 검사하는 메서드.
        // 요약: nav mesh path를 계산해, 마지막 corner와 목표 지점의 거리를 비교함으로서 도달 가능한 위치인지 판별한다.
        // 1. path 계산
        // 2. path의 마지막 점 last 구하기
        // 3. last와 goal이 근접하면 true, 아니면 false 반환

        // (1) path 계산
        if (_agent == null) MyDebug.Log("A");

        bool isProperPath = _agent.CalculatePath(goal, _path);
        if (isProperPath == false)
        {
            return false;
        }

        // (2) path의 마지막 점 last 구하기
        Vector3[] corners = _path.corners;  // ##### 이 코드 불안정함(Vector[]가 자주 생성됨.) #####
        Vector3 last = corners[corners.Length - 1];

        // (3) last와 goal이 근접하면 true, 아니면 false 반환
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
