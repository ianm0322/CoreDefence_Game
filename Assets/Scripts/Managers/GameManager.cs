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

    public GameObject TEST_WEAPON;
    public Sprite TEST_SPRITE;

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
            var weapon = new InventoryItem_Weapon(TEST_WEAPON, "Rifle", TEST_SPRITE);
            InventoryManager.Instance.Inventory.AcquireItem(weapon);
        }
    }
}
