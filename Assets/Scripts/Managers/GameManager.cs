using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public PlayerController player;
    public CoreScript Core;

    public Vector3 CorePosition { get { return Core.transform.position; } }

    public Transform PlayerSpawnPoint;
    public EnemySpawner[] Spawners;

    public ItemObject[] Items;

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
        player.Spawn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < Items.Length; i++)
            {
                InventoryManager.Instance.AddItem(Items[i]);
            }
        }
    }
}
