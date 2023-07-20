using System;
using UnityEngine;

public class ItemObject_Facility : IItem, ICountableItem, ILinkedItem
{
    public int Id { get; }
    public int Count { get; set; }
    public InventoryItemType ItemType { get; private set; }
    public Sprite ItemIcon { get; private set; }

    private GameObject _linkedPrefab;

    public event Action<IItem> ItemDestroyedEvent;

    public ItemObject_Facility(InventoryItemType itemType, Sprite itemIcon, GameObject obj)
    {
        this.ItemType = itemType;
        this.ItemIcon = itemIcon;
        this.SetPrefab(obj);
    }

    public void CancleItem()
    {
        BuilderManager.Instance.StopBuildMode();
        BuilderManager.Instance.OnFacilityBuiltEvent -= OnFacilityBuiltListener;
    }

    public void SetPrefab(GameObject prefab)
    {
        this._linkedPrefab = prefab;
    }

    public void UseItem()
    {
        StartBuildMode();
    }

    private void OnFacilityBuiltListener(GameObject obj)
    {
        DecreaseCount(1);
    }

    private void StartBuildMode()
    {
        BuilderManager.Instance.SetPrefab(_linkedPrefab);
        BuilderManager.Instance.StartBuildMode();
        BuilderManager.Instance.OnFacilityBuiltEvent += OnFacilityBuiltListener;
        // Comment(7.21)
        // 현재 인벤토리 아이템의 보관과 사용이 분리되어 있어, 인벤토리 조작 외의 수단으로 사용되는 아이템의 인벤토리 조작이 조금 난감함.
        // 특히 건설 시스템의 경우에는 매니저를 경유하는데, 매니저를 수정하는 일은 피하고 싶음.
        // 고려중인 해결책은 세 가지.
        // 1. 현재 방법. 매니저는 이벤트로 건설 사실을 공지하고, 인벤토리 아이템이 이벤트를 받아 처리.
        // 2. 인벤토리 외에도 모든 '사용 가능한 아이템'에 대한 기능확장. 단, 이 경우 이미 만든 웨폰 스크립트까지 수정해야 하기에 대대적 수정이 필요할 것.
        // 3. 매니저를 수정하는 대신, 매니저와 아이템 사이에 중간자를 넣는 방법? 발상은 해봤는데 자세히 고려는 안 해봐서 모르겠음.
        // 일단 현재 방법(1)로 진행하고, 이후 진행 상황에 따라 수정하거나 하자.
        // (개인적으로 생각하는 최고의 방법은 2번, 장착된 아이템 클래스를 별도로 만들어 무기, 설치물 클래스로 확장하는 것. 이렇게 되면 ItemObject 클래스도 상당부분 수정해야 할듯.)
    }

    private void DecreaseCount(int value)
    {
        Count -= value;
        if(Count <= 0)
        {
            ItemDestroyedEvent(this);
        }
    }
}