using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CTType;

public class EntityManager : MonoSingleton<EntityManager>
{
    [System.Serializable]
    private class EnemyPrefabNode
    {
        public EnemyKind kind;
        public EnemyAI prefab;
    }

    // Bullet Object Pool
    private ManagedObjectPool<BulletBase> bulletPool;
    private List<BulletBase> liveBulletList = new List<BulletBase>();
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
        bulletPool = new ManagedObjectPool<BulletBase>(bulletPrefab.GetComponent<BulletBase>(), this.transform, 100);
        InitEnemyPool();
    }

    private void FixedUpdate()
    {
        bulletPool.OnFixedUpdate();
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

    #region Bullet object pool methods

    // create
    public BulletBase CreateBullet(BulletData data)
    {
        BulletBase bullet = bulletPool.CreateObject(data);
        liveBulletList.Add(bullet);
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
        bulletPool.PushObject(bullet);
        liveBulletList.Remove(bullet);
        return;
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
    #region For BT
    //private void InitEnemyPool()
    //{
    //    _enemyPoolDict = new Dictionary<EnemyKind, ManagedObjectPool<EnemyAI>>();
    //    foreach (var enemy in enemyPrefabList)
    //    {
    //        _enemyPoolDict.Add(enemy.kind, new ManagedObjectPool<EnemyAI>(enemy.prefab, this.transform));
    //    }
    //}

    //public EnemyAI CreateEnemy(EnemyKind kind, EnemyData data)
    //{
    //    EnemyAI enemy = _enemyPoolDict[kind].CreateObject(data);
    //    return enemy;
    //}

    //public void DestroyEnemy(EnemyAI returnInstance)
    //{
    //    EnemyKind kind = returnInstance.Kind;
    //    _enemyPoolDict[kind].PushObject(returnInstance);
    //}

    //public int GetLiveEnemyCount()
    //{
    //    int liveCount = 0;
    //    foreach (var pool in _enemyPoolDict.Values)
    //    {
    //        liveCount += pool.ActiveList.Count;
    //    }
    //    return liveCount;
    //}
    #endregion For BT
    #endregion [Enemy Object Pool]
}
