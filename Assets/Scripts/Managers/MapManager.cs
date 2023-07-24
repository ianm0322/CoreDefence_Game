using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("제거할 예정인 매니저")]
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

    #region 맵 생성
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
                GameObject obj = Instantiate(FloorPrefab);  // 바닥 생성
                MapFloorObjects[x_iter, y_iter] = obj;      // 배열 저장
                obj.transform.parent = Parent;              // 바닥 부모 설정
                _vector.Set(ElementSize, 1, ElementSize);
                obj.transform.localScale = _vector;
                obj.name = $"Floor {{{x_iter}, {y_iter}}}"; // 바닥 이름 설정

                _vector.Set((x_iter + 0.5f) * ElementSize, 0, (y_iter + 0.5f) * ElementSize);  // 위치 지정
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
        _wall = Instantiate(WallPrefab);                // 생성
        _wall.transform.parent = Parent;                // 부모 설정
        _wall.name = "North Wall";                      // 이름 설정

        _wallScale = MapBoxScale;                       // 크기 설정
        _wallScale.z = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // 위치 설정
        _wallPos.z += (MapBoxScale.z + _wallScale.z) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;

        // South
        _wall = Instantiate(WallPrefab);                // 생성
        _wall.transform.parent = Parent;                // 부모 설정
        _wall.name = "South Wall";                      // 이름 설정

        _wallScale = MapBoxScale;                       // 크기 설정
        _wallScale.z = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // 위치 설정
        _wallPos.z -= (MapBoxScale.z + _wallScale.z) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;

        // East
        _wall = Instantiate(WallPrefab);                // 생성
        _wall.transform.parent = Parent;                // 부모 설정
        _wall.name = "East Wall";                      // 이름 설정

        _wallScale = MapBoxScale;                       // 크기 설정
        _wallScale.x = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // 위치 설정
        _wallPos.x += (MapBoxScale.x + _wallScale.x) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;

        // West
        _wall = Instantiate(WallPrefab);                // 생성
        _wall.transform.parent = Parent;                // 부모 설정
        _wall.name = "West Wall";                      // 이름 설정

        _wallScale = MapBoxScale;                       // 크기 설정
        _wallScale.x = 1;
        _wall.transform.localScale = _wallScale;

        _wallPos = OriginPosition;                      // 위치 설정
        _wallPos.x -= (MapBoxScale.x + _wallScale.x) * 0.5f;
        _wallPos.y += _wallScale.y * 0.5f + 0.5f;
        _wall.transform.position = _wallPos;
    }
    #endregion

    #region 맵 유틸리티
    public Vector3 GetGridPosition(Vector3 position)
    {
        if ((MapCount.x & 1) == 0)   // x가 짝수면
            position.x = (Mathf.Round((position.x - ElementSize * 0.5f) * DivideByElementSize) + 0.5f) * ElementSize;
        else
            position.x = (Mathf.Round((position.x) * DivideByElementSize)) * ElementSize;
        
        if ((MapCount.y & 1) == 0)   // x가 짝수면
            position.z = (Mathf.Round((position.z - ElementSize * 0.5f) * DivideByElementSize) + 0.5f) * ElementSize;
        else
            position.z = (Mathf.Round((position.z) * DivideByElementSize)) * ElementSize;
        
        position.y = (Mathf.Round((position.y - ElementSize * 0.5f) * DivideByElementSize) + 0.5f) * ElementSize;
        return position;
    }
    #endregion
}
