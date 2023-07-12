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

    // Bullet Object Pool
    [SerializeField]
    private List<BulletPrefabNode> _bulletPrefabList;
    private Dictionary<BulletKind, ManagedObjectPool<BulletBase>> _bulletPoolDict;
    

    [Obsolete]
    private ManagedObjectPool<BulletBase> bulletPool;
    [Obsolete]
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private List<EnemyPrefabNode> _enemyPrefabList;
    private Dictionary<EnemyKind, ManagedObjectPool<EnemyAI>> _enemyPoolDict;

    // ################TEST
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        //bulletPool = new ManagedObjectPool<BulletBase>(bulletPrefab.GetComponent<BulletBase>(), this.transform, 100); [Old version]
        InitBulletPool();
        InitEnemyPool();
    }

    private void FixedUpdate()
    {
        //bulletPool.OnFixedUpdate(); [Old version]
        foreach (var pool in _bulletPoolDict.Values)
        {
            pool.OnFixedUpdate();
        }

        foreach (var pool in _enemyPoolDict.Values)
        {
            pool.OnFixedUpdate();
        }
    }

    private void Update()
    {
        foreach (var pool in _enemyPoolDict.Values)
        {
            pool.OnUpdate();
        }
    }

    private void LateUpdate()
    {
        foreach (var pool in _enemyPoolDict.Values)
        {
            pool.OnLateUpdate();
        }
    }

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
