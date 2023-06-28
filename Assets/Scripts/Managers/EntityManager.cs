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
        public EnemyAIController prefab;
        //public EnemyAI prefab; for BT
    }

    // Bullet Object Pool
    private ManagedObjectPool<BulletScript> bulletPool;
    private List<BulletScript> liveBulletList = new List<BulletScript>();
    [SerializeField]
    private GameObject bulletPrefab;

    //[SerializeField]
    //private List<EnemyPrefabNode> enemyPrefabList;
    //private Dictionary<EnemyKind, ManagedObjectPool<EnemyAI>> _enemyPoolDict;
    [SerializeField]
    private List<EnemyPrefabNode> enemyPrefabList;
    private Dictionary<EnemyKind, ManagedObjectPool<EnemyAIController>> _enemyPoolDict;

    // ################TEST
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        bulletPool = new ManagedObjectPool<BulletScript>(bulletPrefab.GetComponent<BulletScript>(), this.transform, 100);
        InitEnemyPool();
    }

    private void FixedUpdate()
    {
        bulletPool.OnFixedUpdate();
    }

    #region Bullet object pool methods

    // create
    public BulletScript CreateBullet(BulletData data)
    {
        BulletScript _bullet = bulletPool.CreateObject(data);
        liveBulletList.Add(_bullet);
        return _bullet;
    }
    public BulletScript CreateBullet(BulletData data, Vector3 position, Quaternion rotation)
    {
        // 만들고
        BulletScript bullet = CreateBullet(data);
        bullet.transform.position = position;   // set position
        bullet.transform.rotation = rotation;   // set
        return bullet;
    }
    public BulletScript CreateBullet(BulletData data, Transform transform)
    {
        return CreateBullet(data, transform.position, transform.rotation);
    }

    // destroy
    public void DestroyBullet(BulletScript bullet)
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
    /// 
    private void InitEnemyPool()
    {
        _enemyPoolDict = new Dictionary<EnemyKind, ManagedObjectPool<EnemyAIController>>();
        foreach (var enemy in enemyPrefabList)
        {
            _enemyPoolDict.Add(enemy.kind, new ManagedObjectPool<EnemyAIController>(enemy.prefab, this.transform));
        }
    }

    public EnemyAIController CreateEnemy(EnemyKind kind, EnemyData data)
    {
        EnemyAIController enemy = _enemyPoolDict[kind].CreateObject(data);
        return enemy;
    }

    public void DestroyEnemy(EnemyAIController returnInstance)
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
