using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이 씬 시작~종료까지 유지되며 게임 객체를 관리하는 매니저
/// </summary>
// - 플레이어
// - 인벤토리
public class StageManager : MonoSingleton<StageManager>
{
    public PlayerController Player { get; private set; }
    public CoreScript Core;

    public Vector3 CorePosition { get { return Core.transform.position; } }

    public Transform PlayerSpawnPoint;
    public EnemySpawner[] EnemySpawnPoint;

    public ItemObjectInfo[] StartingItemBundle;

    #region [Inventory Properties]
    private ItemInventory _inventory;
    public ItemInventory Inventory => _inventory;

    [SerializeField]
    private int _money;
    public int Money => _money;
    #endregion

    public override void InitOnSceneLoad(string sceneName)
    {
        if (sceneName == "PlayScene")
        {
            InitOnPlay();
        }
    }

    public void InitOnPlay()
    {
        MyDebug.Log("StageManager is initialized!");
        InitInventory();
        InitObjects();
    }

    private void InitObjects()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        }
        if (Core == null)
        {
            Core = GameObject.FindGameObjectWithTag("Core")?.GetComponent<CoreScript>();
        }
        if (PlayerSpawnPoint == null)
        {
            PlayerSpawnPoint = GameObject.Find(StaticData.PlayerSpawnPointName)?.transform;
            if (PlayerSpawnPoint == null)
                Debug.LogError($"StageManager: \"{StaticData.PlayerSpawnPointName}\" found failure");
        }
        if (EnemySpawnPoint == null || EnemySpawnPoint.Length == 0)
        {
            EnemySpawnPoint = GameObject.FindObjectsOfType<EnemySpawner>();
        }
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
            AddItem(StartingItemBundle[i]);
            MyDebug.Log($"Give player {StartingItemBundle[i].name}");
        }
    }


    #region [Inventory Methods]
    private void InitInventory()
    {
        _inventory = new ItemInventory();
    }

    public bool AddItem(ItemObjectInfo item)
    {
        ItemInventorySlot slot;
        if (_inventory.TryFindItemSlot(item.type, out slot))
        {
            if (slot.IncreaseItemCount(item.count))
            {
                return true;
            }
        }

        return _inventory.AddItem(item.ConvertToItemData());
    }

    public void AddMoney(int value)
    {
        _money += value;
    }

    public void SubtractMoney(int value)
    {
        _money = _money > value ? _money - value : 0;
    }

    public bool CanSpendMoney(int price)
    {
        return Money >= price;
    }
    #endregion
}
