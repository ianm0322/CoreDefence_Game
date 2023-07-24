using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
    public ItemInventory inventory;

    // Properties
    [Range(0f, 360f)]
    public float Offset;
    public Sprite DefaultSprite;
    public Image Prefab;

    // Field
    public bool IsInventoryOpened { get; private set; }
    public int SelectedIndex { get; set; }
    public int CursoredIndex { get; private set; }

    private Coroutine _mainCoroutine;
    private Coroutine _effectCoroutine;
    private Image[] _icons = null;
    private TMP_Text[] _countText = null;
    private Vector3[] _slotPositions = null;

    private void Start()
    {
        inventory = InventoryManager.Instance.Inventory;

        // Init arrays
        InitUIElements();
        InitSlotPositions();
    }

    private void Update()
    {
        //InventoryControlUpdate();
    }

    private void ImageUpdate()
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            var slot = inventory.GetSlot(i);
            var item = slot?.Item;
            if (item != null)
            {
                _icons[i].sprite = item.ItemIcon;
                _countText[i].text = (item as ICountableItem)?.Count.ToString();
            }
            else
            {
                _icons[i].sprite = DefaultSprite;
                _countText[i].text = null;
            }
        }
    }

    [Obsolete("테스트용 임시 메서드.")]
    private void InventoryControlUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenInventory();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            CloseInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // 아이템 버리기 기능
        }
    }

    #region Interface
    public void SelectSlot(int index)
    {
        if (index != -1)
        {
            inventory.GetSlot(SelectedIndex).Item?.CancleItem();
            SelectedIndex = index;
            inventory.GetSlot(SelectedIndex).Item?.UseItem();
        }
    }

    //public void SelectPointedSlot(out InventoryUIScript slot)
    //{
    //    // 인벤토리가 열려있을 때, 현재 가리키고 있는 슬롯을 선택하는 메서드
    //    slot = null;
    //}

    //public void SelectPointedSlot()
    //{
    //    // 인벤토리가 열려있을 때, 현재 가리키고 있는 슬롯을 선택하는 메서드
    //}

    //public Image HoldSlot()
    //{
    //    return null;
    //}

    //public void PutSlot()
    //{
    //    // Held slot을 현재 가리키는 위치로 이동시킨다.
    //}

    /// <summary>
    /// 인벤토리를 연다.
    /// </summary>
    public void OpenInventory()
    {
        StateInitialize(true);
        ExecuteCoroutine(ref _mainCoroutine, InventoryOpenUpdateCor);
        ExecuteCoroutine(ref _effectCoroutine, InventoryFadeInCor);
    }

    /// <summary>
    /// 인벤토리를 닫는다.
    /// </summary>
    public void CloseInventory()
    {
        SelectSlot(CursoredIndex);

        StateInitialize(false);
        ExecuteCoroutine(ref _mainCoroutine, InventoryCloseCor);
        if (_effectCoroutine != null) StopCoroutine(_effectCoroutine);
    }
    #endregion

    #region Sub method
    #region Initializer    
    /// <summary>
    /// Slot을 표시할 Image Object을 생성하고 배열에 저장한다.
    /// </summary>
    private void InitUIElements()
    {
        if (_icons == null)
        {
            _icons = new Image[inventory.Count];
            _countText = new TMP_Text[inventory.Count];
        }

        for (int i = 0; i < _icons.Length; i++)
        {
            var obj = Instantiate(Prefab);
            _icons[i] = obj.GetComponent<Image>();
            _countText[i] = obj.GetComponentInChildren<TMP_Text>();
            obj.transform.SetParent(this.transform);
            obj.transform.localScale = Vector3.one;
            if (inventory.GetSlot(i).Item != null)
            {
                _icons[i].sprite = inventory.GetSlot(i).Item.ItemIcon;
            }
            else
            {
                _icons[i].sprite = DefaultSprite;
            }
        }
    }
    /// <summary>
    /// 인벤토리 열람 시 Image Object별로 배치될 위치를 설정하고 배열에 저장한다.
    /// </summary>
    private void InitSlotPositions()
    {
        if (_slotPositions == null)
            _slotPositions = new Vector3[inventory.Count];

        for (int i = 0, end = _slotPositions.Length; i < end; i++)
        {
            // 원점+원주 상 위치로 설정.
            _slotPositions[i] = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);
            _slotPositions[i] += new Vector3(
                            Mathf.Sin(Mathf.PI * 2f * (float)i / end),
                            Mathf.Cos(Mathf.PI * 2f * (float)i / end)
                            ) * Screen.height / 3;
        }
    }
    #endregion

    /// <summary>
    /// 이전에 실행되던 코루틴을 중단하고 새로운 코루틴을 실행시키는 메서드.
    /// </summary>
    /// <param name="val"></param>
    /// <param name="coroutine"></param>
    private void ExecuteCoroutine(ref Coroutine val, Func<IEnumerator> coroutine)
    {
        if (val != null)
            StopCoroutine(val);
        val = StartCoroutine(coroutine());
    }
    /// <summary>
    /// 화면 중앙에서 마우스 위치까지의 벡터를 반환한다.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetOriginPointToMouseVector()
    {
        Vector2 mouse = Input.mousePosition;
        Vector2 p_o = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);
        mouse = mouse - p_o;
        return mouse;
    }
    /// <summary>
    /// 원점에서부터 마우스 위치까지의 각도를 반환한다. vector.up = 0도, vector.right = 90도
    /// </summary>
    /// <param name="mouse"></param>
    /// <returns></returns>
    private float GetMouseAngle(Vector2 mouse)
    {
        mouse.Normalize();
        float angle = Mathf.Repeat(Mathf.Atan2(mouse.x, mouse.y) * Mathf.Rad2Deg, 360f);
        return angle;
    }
    /// <summary>
    /// 각도를 n등분하여, 그 중 몇 번째 분절에 있는지 index로 반환한다.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private int AngleToIndex(float angle)
    {
        /*
         * [해설]
         * 1. angle = angle + offset    => 보정값 적용
         * 2. angle / 360               => angle = 0..1
         * 3. angle * n                 => index = 0~n
         * 4. index %= n                => index는 0~n-1 사이값이므로, index==n일때 0으로 만들어줌.
         */
        return (Mathf.RoundToInt(((angle + Offset) / 360f) * (inventory.Count))) % (inventory.Count);
    }
    /// <summary>
    /// 마우스 위치가 중앙으로부터 가까운 위치에 있으면 false를 반환한다.
    /// </summary>
    /// <param name="mouse"></param>
    /// <returns></returns>
    private bool CheckSelectionDist(Vector2 mouse)
    {
        return mouse.magnitude > Screen.height / 10;
    }
    /// <summary>
    /// 마우스의 위치에 따라 선택된 인벤토리 슬롯의 인덱스를 반환한다.
    /// </summary>
    /// <returns></returns>
    private int GetSelectionIndex()
    {
        Vector2 mouse = GetOriginPointToMouseVector();
        if (CheckSelectionDist(mouse))
        {
            float angle = GetMouseAngle(mouse);
            return AngleToIndex(angle);
        }
        else
        {
            return -1;
        }
    }
    /// <summary>
    /// 선택된 슬롯을 강조한다.
    /// </summary>
    /// <param name="selectedIdx"></param>
    private void HighlightSelection(int selectedIdx)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            if (i == selectedIdx)
            {
                _icons[i].transform.localScale = Vector3.one * 1.2f; // 선택된 슬롯 크기 1.2배 키움.
            }
            else
            {
                _icons[i].transform.localScale = Vector3.one;
            }
        }
    }
    /// <summary>
    /// 인벤토리 open/close 상태에 따라 필요한 초기화를 해주는 메서드.
    /// </summary>
    /// <param name="isOpened"></param>
    private void StateInitialize(bool isOpened)
    {
        Cursor.lockState = isOpened ? CursorLockMode.Confined : CursorLockMode.Locked;
        IsInventoryOpened = isOpened;
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].gameObject.SetActive(isOpened);
        }
    }

    // Fade inout Methods
    /// <summary>
    /// 슬롯 위치를 원점(화면 중앙)에 갖다 놓는다.
    /// </summary>
    private void SetSlotPositionToZeroPoint()
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].rectTransform.position = _icons[i].rectTransform.position = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);
        }
    }
    /// <summary>
    /// 슬롯의 위치 보간값을 반환한다.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector3 FadeInLerp(Image target, Vector3 position)
    {
        return Vector3.Lerp(target.rectTransform.position, position, Time.deltaTime * 5f);
    }
    /// <summary>
    /// 슬롯 위치를 최신화시킨다.
    /// </summary>
    private void UpdateSlotPositions()
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].rectTransform.position = FadeInLerp(_icons[i], _slotPositions[i]);
        }
    }
    #endregion

    #region Main Logic
    /// <summary>
    /// 인벤토리 열 때 연출 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator InventoryFadeInCor()
    {
        float time = Time.time;
        yield return null;

        SetSlotPositionToZeroPoint();   // 슬롯 위치를 원점에 맞춤

        while (true)
        {
            UpdateSlotPositions();      // 슬롯 위치 업데이트

            if (Time.time - time > 5f)   // 연출 코루틴 정지 코드
            {
                _effectCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }
    /// <summary>
    /// 인벤토리가 열려있는 동안 작동할 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator InventoryOpenUpdateCor()
    {
        yield return null;

        RectTransform heldTr = null;
        int heldIndex = -1;

        while (true)
        {
            ImageUpdate();

            // 아이템 옮기기 코드

            // 1. 아이콘에 마우스 클릭 시
            if (Input.GetMouseButtonDown(0) && CursoredIndex != -1)
            {
                heldIndex = CursoredIndex;
                heldTr = _icons[heldIndex].rectTransform;
                heldTr.SetAsLastSibling();
            }
            else if (Input.GetMouseButton(0) && heldTr != null)
            {
                if (heldTr != null)
                {
                    heldTr.position = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (heldTr != null)
                {
                    if (CursoredIndex != -1 && heldIndex != CursoredIndex)
                    {
                        Debug.Log("Swap");
                        inventory.SwapSlotPosition(heldIndex, CursoredIndex);
                        ImageUpdate();
                    }
                    heldTr.position = _slotPositions[heldIndex];
                    heldTr = null;
                }
            }

            CursoredIndex = GetSelectionIndex();    // 선택된 슬롯 인덱스 설정
            HighlightSelection(CursoredIndex);      // 선택된 슬롯 하이라이트

            yield return null;
        }
    }
    /// <summary>
    /// 인벤토리가 닫힐 때.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InventoryCloseCor()
    {
        yield return null;
        _mainCoroutine = null;
    }
    #endregion
}
