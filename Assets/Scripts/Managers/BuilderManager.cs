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
    /// ��ġ�Ϸ��� ��ġ�� �ٸ� ������Ʈ�� ���� �����ϴ��� �˻�.
    /// </summary>
    /// <returns></returns>
    public bool IsNotCollisionWithObstacle()
    {
        Collider[] cols = new Collider[2];  // �ϳ��� �浹�ص� false�̹Ƿ� size�� 2. (cols[0]�� �׻� �ڱ� �ڽ�.)
        Physics.OverlapBoxNonAlloc(GuideTr.position, GuideTr.localScale * 0.49999f, cols, GuideTr.rotation, LayerMask.GetMask("Wall", "Facility")); // ���� ��ġ���� ������.
        return cols[1] == null; // �浹�� �� ������ true ��ȯ
    }

    /// <summary>
    /// Guide Tr�� ���� ��ġ ������ ����� ������ �����ϴ��� �˻�.
    /// </summary>
    /// <returns></returns>
    public bool IsEnoughPlace()
    {
        ray.direction = Vector3.down;   // ���̴� ��� �Ʒ� �������� �˻�.
        for (int i = 0; i < 4; i++) // ��ġ ������ �� ������ ��� ���� ��������� ��ġ ������ ��ġ��.
        {
            Vector3 point = GuideRayPoints[i].position;
            ray.origin = point;
            if (Physics.Raycast(ray, 0.01f, LayerMask.GetMask("Wall")) == false)    // �� ������ �� �ϳ��� ���߿� �������� false ��ȯ
            {
                return false;
            }
        }
        return true;// �� ������ ��� ���� ������ true ��ȯ
    }

    #region Builder utilities
    public Vector3 GetBuildPosition(Vector3 pos)
    {
        pos.x = Mathf.Round(pos.x / facilitiesStandardUnit) * facilitiesStandardUnit;
        pos.z = Mathf.Round(pos.z / facilitiesStandardUnit) * facilitiesStandardUnit;

        return pos;
    }

    /// <summary>
    /// �ش� ��ġ�� �ǹ��� ��ġ�� �� ������ true ��ȯ, ��ġ �Ұ��� false ��ȯ
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
