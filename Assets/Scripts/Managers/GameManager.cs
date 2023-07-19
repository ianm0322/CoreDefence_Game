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

    public ItemObjectInfo[] Items;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            var obj = GetGazeObject(Camera.main, LayerMask.GetMask("UI"));
            if(obj != null)
            {
                IInteractable interact;
                if(obj.TryGetComponent(out interact))
                {
                    interact.Interact();
                }
            }
        }
    }

    GameObject GetGazeObject(Camera cam, LayerMask layer)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, float.PositiveInfinity, layer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
