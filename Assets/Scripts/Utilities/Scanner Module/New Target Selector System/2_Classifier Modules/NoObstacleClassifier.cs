using UnityEngine;

public class NoObstacleClassifier : AbstractClassifier
{
    private Transform _self;
    private LayerMask _obstacleLayer;

    protected override bool Check(Collider target)
    {
        // 대상과 나 사이에 가로막는 장애물이 있으면 실패 반환
        if (IsObstacleInBetween(target.transform) == true)
        {
            return false;
        }
        // 없으면 성공 반환
        else
        {
            return true;
        }
    }

    private bool IsObstacleInBetween(Transform obj)
    {
        // 대상과 나 사이를 가로막는 장애물이 있는가?
        if (Physics.Linecast(_self.position, obj.position, _obstacleLayer)) 
        {
            //Debug.DrawLine(_self.position, obj.position, Color.red);
            return true;
        }
        else
        {
            //Debug.DrawLine(_self.position, obj.position, Color.green);
            return false;
        }
    }
}