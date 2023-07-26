using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static CTType;

public class EntityManager : MonoSingleton<EntityManager>
{
    [Serializable]
    private class EnemyPrefabNode
    {
        public EnemyKind kind;
        public EnemyAI prefab;
    }

    [Serializable]
    private class BulletPrefabNode
    {
        public BulletKind kind;
        public BulletBase prefab;
    }

    [Serializable]
    private class FacilityPrefabNode
    {
        public FacilityKind kind;
        public FacilityAI prefab;
    }

    // Bullet Object Pool
    [SerializeField]
    private List<BulletPrefabNode> _bulletPrefabList;
    private Dictionary<BulletKind, ManagedObjectPool<BulletBase>> _bulletPoolDict;

    [SerializeField]
    private List<EnemyPrefabNode> _enemyPrefabList;
    private Dictionary<EnemyKind, ManagedObjectPool<EnemyAI>> _enemyPoolDict;

    [SerializeField]
    private List<FacilityPrefabNode> _facilityPrefabList;
    private Dictionary<FacilityKind, ManagedObjectPool<FacilityAI>> _facilityPoolDict;

    // ################TEST
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        //bulletPool = new ManagedObjectPool<BulletBase>(bulletPrefab.GetComponent<BulletBase>(), this.transform, 100); [Old version]
        //InitBulletPool();
        //InitEnemyPool();
        //InitFacilityPool();
    }

    public override void InitOnSceneLoad(string sceneName)
    {
        if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            InitBulletPool();
            InitEnemyPool();
            InitFacilityPool();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            if (_enemyPoolDict == null)
                return;
            foreach (var pool in _bulletPoolDict.Values)
            {
                pool.OnFixedUpdate();
            }

            foreach (var pool in _enemyPoolDict.Values)
            {
                pool.OnFixedUpdate();
            }

            foreach (var pool in _facilityPoolDict.Values)
            {
                pool.OnFixedUpdate();
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            if (_enemyPoolDict == null)
                return;
            foreach (var pool in _enemyPoolDict.Values)
            {
                pool.OnUpdate();
            }

            foreach (var pool in _facilityPoolDict.Values)
            {
                pool.OnUpdate();
            }
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.GetCurrentScene() == Data.SceneKind.PlayScene)
        {
            if (_enemyPoolDict == null)
                return;
            foreach (var pool in _enemyPoolDict.Values)
            {
                pool.OnLateUpdate();
            }
        }
    }

    public void ClearAllPools()
    {
        foreach (var pool in _bulletPoolDict.Values)
        {
            pool.Clear();
        }

        foreach (var pool in _enemyPoolDict.Values)
        {
            pool.Clear();
        }

        foreach (var pool in _facilityPoolDict.Values)
        {
            pool.Clear();
        }
    }

    #region [Facility object pool]
    private void InitFacilityPool()
    {
        _facilityPoolDict = new Dictionary<FacilityKind, ManagedObjectPool<FacilityAI>>();
        for (int i = 0; i < _facilityPrefabList.Count; i++)
        {
            _facilityPoolDict.Add(_facilityPrefabList[i].kind, new ManagedObjectPool<FacilityAI>(_facilityPrefabList[i].prefab, this.transform));
        }
    }

    public FacilityAI CreateFacility(FacilityData data)
    {
        var entity = _facilityPoolDict[data.Kind].CreateObject(data);
        return entity;
    }

    public FacilityAI CreateFacility(FacilityData data, Vector3 position, Quaternion rotation)
    {
        var entity = CreateFacility(data);
        entity.transform.position = position;
        entity.transform.rotation = rotation;
        return entity;
    }
    public FacilityAI CreateFacility(FacilityData data, Transform transform)
    {
        return CreateFacility(data, transform.position, transform.rotation);
    }

    // destroy
    public void DestroyFacility(FacilityAI entity)
    {
        _facilityPoolDict[entity.Data.Kind].PushObject(entity);
        return;
    }
    #endregion

    #region [Bullet object pool methods]

    // create
    public BulletBase CreateBullet(BulletData data)
    {
        //BulletBase bullet = bulletPool.CreateObject(data);    [Old version]
        BulletBase bullet = _bulletPoolDict[data.type].CreateObject(new BulletData(data));
        return bullet;
    }

    public BulletBase CreateBullet(BulletData data, Vector3 position, Quaternion rotation)
    {
        // 만들고
        BulletBase bullet = CreateBullet(data);
        bullet.transform.position = position;   // set position
        bullet.transform.rotation = rotation;   // set
        return bullet;
    }
    public BulletBase CreateBullet(BulletData data, Transform transform)
    {
        return CreateBullet(data, transform.position, transform.rotation);
    }

    // destroy
    public void DestroyBullet(BulletBase bullet)
    {
        //bulletPool.PushObject(bullet); [Old version]
        _bulletPoolDict[bullet.Data.type].PushObject(bullet);
        return;
    }

    private void InitBulletPool()
    {
        _bulletPoolDict = new Dictionary<BulletKind, ManagedObjectPool<BulletBase>>();
        for (int i = 0; i < _bulletPrefabList.Count; i++)
        {
            _bulletPoolDict.Add(_bulletPrefabList[i].kind, new ManagedObjectPool<BulletBase>(_bulletPrefabList[i].prefab, this.transform));
        }
    }
    #endregion

    #region [Enemy Object Pool]
    /// <summary>
    /// _enemyPoolDict 딕셔너리를 enemyPrefabList의 입력값으로 초기화하는 메서드.
    /// </summary>
    private void InitEnemyPool()
    {
        _enemyPoolDict = new Dictionary<EnemyKind, ManagedObjectPool<EnemyAI>>();
        foreach (var enemy in _enemyPrefabList)
        {
            _enemyPoolDict.Add(enemy.kind, new ManagedObjectPool<EnemyAI>(enemy.prefab, this.transform));
        }
    }

    public EnemyAI CreateEnemy(EnemyKind kind, EnemyData data)
    {
        EnemyAI enemy = _enemyPoolDict[kind].CreateObject(data);
        return enemy;
    }

    public void DestroyEnemy(EnemyAI returnInstance)
    {
        EnemyKind kind = returnInstance.Kind;
        _enemyPoolDict[kind].PushObject(returnInstance);
    }

    public int GetLiveEnemyCount()
    {
        int liveCount = 0;
        foreach (var pool in _enemyPoolDict.Values)
        {
            liveCount += pool.ActiveList.Count;
        }
        return liveCount;
    }
    #endregion [Enemy Object Pool]
}
