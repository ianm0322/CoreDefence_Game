using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoManager : MonoSingleton<StageInfoManager>
{
    public PlayerController Player { get; private set; }
    public CoreScript Core;

    public Vector3 CorePosition { get { return Core.transform.position; } }

    public Transform PlayerSpawnPoint;
    public EnemySpawner[] Spawners;

    public ItemObjectInfo[] StartingItemBundle;
    
    protected override void Awake()
    {
        base.Awake();
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        if(Core == null)
        {
            Core = GameObject.FindGameObjectWithTag("Core").GetComponent<CoreScript>();
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Player.Init();
        Player.Spawn();
        GiveStartingItemBundle();
    }

    public GameObject GetGazeObject(Camera cam, LayerMask layer)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, float.PositiveInfinity, layer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    public bool InteractOnGaze()
    {
        var obj = GetGazeObject(Camera.main, LayerMask.GetMask("UI"));
        if (obj != null)
        {
            IInteractable interact;
            if (obj.TryGetComponent(out interact))
            {
                interact.Interact();
                return true;
            }
        }
        return true;
    }

    public void GiveStartingItemBundle()
    {
        for (int i = 0; i < StartingItemBundle.Length; i++)
        {
            InventoryManager.Instance.AddItem(StartingItemBundle[i]);
        }
    }
}
