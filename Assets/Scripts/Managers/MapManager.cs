using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("������ ������ �Ŵ���")]
public class MapManager : MonoSingleton<MapManager>
{
    [Rito.CustomAttributes.MethodButton("GenerateMap", "Generation")]
    public int generation;
    [Rito.CustomAttributes.MethodButton("ClearFloor", "Clear")]
    public int clear;
    [Header("Map Elements")]
    public GameObject FloorPrefab;
    public GameObject WallPrefab;
    public GameObject StructurePrefab;
    public int ElementSize = 1;
    public float DivideByElementSize { get; private set; }

    [Header("Map Data")]
    public Vector2Int MapCount;
    public float MapHeight;
    public Vector3 OriginPosition;
    public Vector3 MapPlaneScale
    {
        get
        {
            _vector.Set(MapCount.x * ElementSize, 0, MapCount.y * ElementSize);
            return _vector;
        }
    }
    public Vector3 MapBoxScale
    {
        get
        {
            _vector.Set(MapCount.x * ElementSize, MapHeight, MapCount.y * ElementSize);
            return _vector;
        }
    }
    public Bounds MapBounds => _mapBounds;
    private Bounds _mapBounds;

    [Header("Option")]
    public Transform Parent;

    // Data
    public GameObject[,] MapFloorObjects;

    // Temp
    private Vector3 _vector;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        DivideByElementSize = 1f / ElementSize;
    }

    #region �� ����
    public void GenerateMap()
    {
        ClearFloor();
        MapCount.x = Mathf.Min(MapCount.x, 100);
        MapCount.y = Mathf.Min(MapCount.y, 100);
        MapFloorObjects = new GameObject[MapCount.x, MapCount.y];
        GenerateFloor();
        GenerateWall();

        SetMapBounds();
        Debug.Log($"Map Generation Successed! : {{{MapPlaneScale.x}, {MapPlaneScale.y}}}");
    }

    public void ClearFloor()
    {
        var children = GetChildrenTransforms();
        if (children.Length > 0)
        {
            Debug.Log($"{Parent}'s children is destroyed: {children.Length}");
            foreach (Transform item in children)
            {
                if (item == Parent)
                    continue;

                DestroyImmediate(item.gameObject);
            }
        }
        else
        {
            Debug.Log($"{Parent.name} has no children.");
        }
    }

    public Transform[] GetChildrenTransforms()
    {
        return Parent.GetComponentsInChildren<Transform>();
    }

    private void SetMapBounds()
    {
        _mapBounds.center = OriginPosition + Vector3.up * MapBoxScale.y * 0.5f;
        _mapBounds.size = MapBoxScale * 0.5f;
    }

    private void GenerateFloor()
    {
        for (int x_iter = 0; x_iter < MapCount.x; x_iter++)
        {
            for (int y_iter = 0; y_iter < MapCount.y; y_iter++)
            {
                GameObject obj = Instantiate(FloorPrefab);  // �ٴ� ����
                MapFloorObjects[x_iter, y_iter] = obj;      // �迭 ����
                obj.transform.parent = Parent;              // �ٴ� �θ� ����
                _vector.Set(ElementSize, 1, ElementSize);
                obj.transform.localScale = _vector;
                obj.name = $"Floor {{{x_iter}, {y_iter}}}"; // �ٴ� �̸� ����

                _vector.Set((x_iter + 0.5f) * ElementSize, 0, (y_iter + 0.5f) * ElementSize);  // ��ġ ����
                obj.transform.position = _vector - (MapPlaneScale * 0.5f) + OriginPosition;
            }
        }
    }

    private void GenerateWall()
    {
        GameObject _wall;
        Vector3 _wallScale;
        Vector3 _wallPos;
        // North
        _wall = Instantiate(WallPrefab);                // ����
        _wall.transform.parent = Parent;                // �θ� ����
        _wall.name = "North Wall";                      // �̸� ����

        _wallScale = MapBoxScale;                       // ũ�� ����
        _wallScale.z = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // ��ġ ����
        _wallPos.z += (MapBoxScale.z + _wallScale.z) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;

        // South
        _wall = Instantiate(WallPrefab);                // ����
        _wall.transform.parent = Parent;                // �θ� ����
        _wall.name = "South Wall";                      // �̸� ����

        _wallScale = MapBoxScale;                       // ũ�� ����
        _wallScale.z = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // ��ġ ����
        _wallPos.z -= (MapBoxScale.z + _wallScale.z) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;

        // East
        _wall = Instantiate(WallPrefab);                // ����
        _wall.transform.parent = Parent;                // �θ� ����
        _wall.name = "East Wall";                      // �̸� ����

        _wallScale = MapBoxScale;                       // ũ�� ����
        _wallScale.x = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // ��ġ ����
        _wallPos.x += (MapBoxScale.x + _wallScale.x) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;

        // West
        _wall = Instantiate(WallPrefab);                // ����
        _wall.transform.parent = Parent;                // �θ� ����
        _wall.name = "West Wall";                      // �̸� ����

        _wallScale = MapBoxScale;                       // ũ�� ����
        _wallScale.x = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // ��ġ ����
        _wallPos.x -= (MapBoxScale.x + _wallScale.x) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;
    }
    #endregion

    #region �� ��ƿ��Ƽ
    public Vector3 GetGridPosition(Vector3 position)
    {
        if ((MapCount.x & 1) == 0)   // x�� ¦����
            position.x = (Mathf.Round((position.x - ElementSize * 0.5f) * DivideByElementSize) + 0.5f) * ElementSize;
        else
            position.x = (Mathf.Round((position.x) * DivideByElementSize)) * ElementSize;
        
        if ((MapCount.y & 1) == 0)   // x�� ¦����
            position.z = (Mathf.Round((position.z - ElementSize * 0.5f) * DivideByElementSize) + 0.5f) * ElementSize;
        else
            position.z = (Mathf.Round((position.z) * DivideByElementSize)) * ElementSize;
        
        position.y = (Mathf.Round((position.y - ElementSize * 0.5f) * DivideByElementSize) + 0.5f) * ElementSize;
        return position;
    }
    #endregion
}
