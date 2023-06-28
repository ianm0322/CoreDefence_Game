using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static int _indexer = 0;

    public GameObject prefab;

    public void Spawn()
    {
        Spawn(prefab);
    }

    public void Spawn(GameObject prefab)
    {
        var obj = GenerateEnemyInstance(prefab);
        obj.Agent.Warp(this.transform.position);
    }

    private EnemyAIController GenerateEnemyInstance(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.name = $"Enemy_{_indexer++}";
        return obj.GetComponent<EnemyAIController>();
    }
}
