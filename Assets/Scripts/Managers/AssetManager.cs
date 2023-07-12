using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CTType;

public class AssetManager : MonoSingleton<AssetManager>
{
    private struct WeaponNode
    {
        public WeaponKind type;
        public GameObject prefab;
    }

    private struct EnemyNode
    {
        public EnemyKind type;
        public GameObject prefab;
    }

    private struct BulletNode
    {
        public BulletKind type;
        public GameObject prefab;
    }

    private struct FacilityNode
    {
        public FacilityKind type;
        public GameObject prefab;
    }

    [SerializeField]
    private List<WeaponNode> _weaponList;
    [SerializeField]
    private List<BulletNode> _bulletList;
    [SerializeField]
    private List<EnemyNode> _enemyList;
    [SerializeField]
    private List<FacilityNode> _facilityList;

    private Dictionary<WeaponKind, GameObject> _weaponDict;
    private Dictionary<BulletKind, GameObject> _bulletDict;
    private Dictionary<EnemyKind, GameObject> _enemyDict;
    private Dictionary<FacilityKind, GameObject> _facilityDict;

    protected override void Awake()
    {
        base.Awake();
        _weaponDict = new Dictionary<WeaponKind, GameObject>();
        for (int i = 0; i < _weaponList.Count; i++)
        {
            _weaponDict.Add(_weaponList[i].type, _weaponList[i].prefab);
        }

        _bulletDict = new Dictionary<BulletKind, GameObject>();
        for (int i = 0; i < _bulletList.Count; i++)
        {
            _bulletDict.Add(_bulletList[i].type, _bulletList[i].prefab);
        }

        _enemyDict = new Dictionary<EnemyKind, GameObject>();
        for (int i = 0; i < _enemyList.Count; i++)
        {
            _enemyDict.Add(_enemyList[i].type, _enemyList[i].prefab);
        }

        _facilityDict = new Dictionary<FacilityKind, GameObject>();
        for (int i = 0; i < _facilityList.Count; i++)
        {
            _facilityDict.Add(_facilityList[i].type, _facilityList[i].prefab);
        }
    }

    public GameObject GetPrefab(WeaponKind type)
    {
        return _weaponDict[type];
    }

    public GameObject GetPrefab(BulletKind type)
    {
        return _bulletDict[type];
    }

    public GameObject GetPrefab(EnemyKind type)
    {
        return _enemyDict[type];
    }

    public GameObject GetPrefab(FacilityKind type)
    {
        return _facilityDict[type];
    }
}


public enum FacilityKind
{
    Shooter,
    Cannon,
    END
}