using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class EntityClassifier_MeleeAgent : EntityClassifier_NearSort
{
    private float _range = 1f;

    private NavMeshAgent _agent;
    private Transform _tr;

    public EntityClassifier_MeleeAgent(Transform origin, NavMeshAgent agent, string[] tags) : base(origin, tags)
    {
        _agent = agent;
        _tr = origin;
    }

    protected override bool Filter(Transform obj)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(obj.transform.position, path);
        Vector3[] corner = path.corners;
        Debug.Log("A");
        if (corner.Length == 0)
            return false;
        Debug.Log("B");
        Vector3 last = corner[corner.Length - 1];

        // 패스의 마지막 점과 타겟 위치가 유사하면 이동 가능.
        // 이동 불가능하면 false 반환
        if(MathUtility.CompareDist(last - obj.transform.position, _range) > 0)
        {
            return false;
        }
        return base.Filter(obj); 
    }
} // public class NearSortClassifier : EntityClassifier
