using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public PlayerController player;
    public CoreScript Core;

    public Vector3 CorePosition { get { return Core.transform.position; } }
    public Vector3[] PathPoint;

    public Transform PlayerSpawnPoint;
    public EnemySpawner[] Spawners;

    protected override void Awake()
    {
        base.Awake();
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        player.Init();
        player.Die();   // ##########################TEST
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void Spawn(int spawnerIndex)
    {
        Spawners[spawnerIndex].Spawn();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < PathPoint.Length-1; i++)
        {
            Gizmos.DrawLine(PathPoint[i], PathPoint[i + 1]);
        }
    }
#endif
}
