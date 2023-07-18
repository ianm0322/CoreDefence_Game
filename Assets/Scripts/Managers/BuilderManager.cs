using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoSingleton<BuilderManager>
{
    public BuildGuiderScript Guide;
    private Transform _guideTr;
    public Transform[] GuideRayPoints;
    public GameObject Prefab;
    private FacilityAI _prefabData;

    [Header("Facilities")]
    public float facilitiesStandardUnit;

    public bool IsBuildMode = false;

    private Coroutine _buildModeCoroutine;
    private Ray ray = new Ray();
    private Collider[] _cols = new Collider[1];  // Guide 충돌 검사용 임시 변수

    protected override void Awake()
    {
        base.Awake();
        _guideTr = Guide.transform;

        GuideRayPoints = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            GuideRayPoints[i] = _guideTr.transform.GetChild(i);
        }

        // TEST
        //Prefab.TryGetComponent(out _prefabData);
        //StartBuildMode();
    }

    public void SetPrefab(GameObject prefab)
    {
        Prefab = prefab;
        Prefab.TryGetComponent(out _prefabData);
        _guideTr.localPosition = _prefabData.facilityScale;
    }

    public void StartBuildMode()
    {
        if(IsBuildMode == false)
        {
            IsBuildMode = true;
            if (_buildModeCoroutine != null)
                StopCoroutine(_buildModeCoroutine);
            _buildModeCoroutine = StartCoroutine(OnBuildModeCoroutine());
        }
    }

    public void StopBuildMode()
    {
        if(_buildModeCoroutine != null)
        {
            IsBuildMode = false;
        }
    }

    private IEnumerator OnBuildModeCoroutine()
    {
        yield return null;
        RaycastHit hit;
        _guideTr.gameObject.SetActive(true);
        while (IsBuildMode)
        {

            _guideTr.localScale = _prefabData.facilityScale;

            if (GetMousePosition(out hit))
            {
                //var v = GetBuildPosition(hit.point + hit.normal * MapManager.Instance.ElementSize * 0.99f);
                //tr.position = v;
                var v = hit.point;
                v.y += _guideTr.localScale.y * 0.5f;
                _guideTr.position = v;
                if (IsPlaceable())
                {
                    _guideTr.GetComponent<Renderer>().material.color = Color.green;

                    if (Input.GetMouseButtonDown(1) &&
                    Physics.OverlapBox(_guideTr.position, _guideTr.localScale * 0.4999f).Length == 0)
                    {
                        Build();
                    }
                }
                else
                {
                    _guideTr.GetComponent<Renderer>().material.color = Color.red;
                }
            }
            
            yield return null;
        }
        _guideTr.gameObject.SetActive(false);
        _buildModeCoroutine = null;
    }

    public void Build()
    {
        var obj = EntityManager.Instance.CreateFacility(_prefabData.Data);
        obj.transform.position = _guideTr.position + Vector3.down * _guideTr.localScale.y * 0.5f;
    }

    public bool GetMousePosition(out RaycastHit hit)
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        return Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask("Wall"));
    }

    public bool IsPlaceable()
    {
        return IsNotCollisionWithObstacle() && IsEnoughPlace();
    }

    /// <summary>
    /// 설치하려는 위치에 다른 오브젝트가 겹쳐 존재하는지 검사.
    /// </summary>
    /// <returns></returns>
    public bool IsNotCollisionWithObstacle()
    {
        _cols[0] = null;
        Physics.OverlapBoxNonAlloc(_guideTr.position, _guideTr.localScale * 0.49999f, _cols, _guideTr.rotation, LayerMask.GetMask("Wall", "Facility")); // 벽과 설치물만 감지함.
        return _cols[0] == null; // 충돌된 게 없으면 true 반환
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
