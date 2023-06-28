using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoSingleton<BuilderManager>
{
    public Transform GuideTr;
    public Transform[] GuideRayPoints;
    public GameObject Prefab;

    [Header("Facilities")]
    public float facilitiesStandardUnit;


    public bool IsBuildMode = false;

    Ray ray = new Ray();

    protected override void Awake()
    {
        base.Awake();


        GuideRayPoints = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            GuideRayPoints[i] = GuideTr.transform.GetChild(i);
        }
    }

    private void Update()
    {
        if (IsBuildMode)
        {
            RaycastHit hit;
            GuideTr.gameObject.SetActive(true);
            if (GetMousePosition(out hit))
            {
                //var v = GetBuildPosition(hit.point + hit.normal * MapManager.Instance.ElementSize * 0.99f);
                //tr.position = v;
                var v = hit.point;
                v.y += GuideTr.localScale.y * 0.5f;
                GuideTr.position = v;
                GuideTr.GetComponent<Renderer>().material.color = (IsNotCollisionWithObstacle() && IsEnoughPlace()) ? Color.green : Color.red;
            }
            if (Input.GetMouseButtonDown(1) &&
                Physics.OverlapBox(GuideTr.position, GuideTr.localScale * 0.49f).Length == 0)
            {
                Build();
            }
        }
        else
        {
            GuideTr.gameObject.SetActive(false);
        }
    }

    public void Build()
    {
        var obj = Instantiate(Prefab);
        obj.transform.position = GuideTr.position;
    }

    public bool GetMousePosition(out RaycastHit hit)
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        return Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask("Wall"));
    }

    /// <summary>
    /// 설치하려는 위치에 다른 오브젝트가 겹쳐 존재하는지 검사.
    /// </summary>
    /// <returns></returns>
    public bool IsNotCollisionWithObstacle()
    {
        Collider[] cols = new Collider[2];  // 하나만 충돌해도 false이므로 size는 2. (cols[0]은 항상 자기 자신.)
        Physics.OverlapBoxNonAlloc(GuideTr.position, GuideTr.localScale * 0.49999f, cols, GuideTr.rotation, LayerMask.GetMask("Wall", "Facility")); // 벽과 설치물만 감지함.
        return cols[1] == null; // 충돌된 게 없으면 true 반환
    }

    /// <summary>
    /// Guide Tr과 비교해 설치 가능한 충분한 공간이 존재하는지 검사.
    /// </summary>
    /// <returns></returns>
    public bool IsEnoughPlace()
    {
        ray.direction = Vector3.down;   // 레이는 모두 아래 방향으로 검사.
        for (int i = 0; i < 4; i++) // 설치 지점의 네 꼭지점 모두 벽에 닿아있으면 설치 가능한 위치임.
        {
            Vector3 point = GuideRayPoints[i].position;
            ray.origin = point;
            if (Physics.Raycast(ray, 0.01f, LayerMask.GetMask("Wall")) == false)    // 네 꼭지점 중 하나라도 공중에 떠있으면 false 반환
            {
                return false;
            }
        }
        return true;// 네 꼭지점 모두 문제 없으면 true 반환
    }

    #region Builder utilities
    public Vector3 GetBuildPosition(Vector3 pos)
    {
        pos.x = Mathf.Round(pos.x / facilitiesStandardUnit) * facilitiesStandardUnit;
        pos.z = Mathf.Round(pos.z / facilitiesStandardUnit) * facilitiesStandardUnit;

        return pos;
    }

    /// <summary>
    /// 해당 위치에 건물을 설치할 수 있으면 true 반환, 설치 불가면 false 반환
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckBuildablePosition(Vector3 pos)
    {
        pos = GetBuildPosition(pos);
        var col = Physics.OverlapBox(pos, Vector3.one * facilitiesStandardUnit * 0.5f);
        return col == null;
    }
    #endregion
}
