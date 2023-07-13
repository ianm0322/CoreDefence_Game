using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public ItemInventory inventory;

    // Properties
    [Range(0f, 360f)]
    public float offset;
    public Sprite defaultSprite;
    public Image prefab;

    // Field
    public bool isInventoryOpened { get; private set; }
    public int selectedIndex { get; set; }

    private Coroutine _mainCoroutine;
    private Coroutine _effectCoroutine;
    private Image[] icons = null;
    private Vector3[] _slotPositions = null;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Init arrays
        InitUIObject();
        InitSlotPositions();
    }

    private void Update()
    {
        InventoryUpdate();
    }

    private void InventoryUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenInventory();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log(selectedIndex);
            CloseInventory();
        }

        else if (Input.GetMouseButtonDown(0))
        {
            // 아이템 옮기기 기능
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // 아이템 버리기 기능
        }
    }

    #region Interface
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
    private void InitUIObject()
    {
        if(icons == null)
        {
            icons = new Image[inventory.Count];
        }

        for (int i = 0; i < icons.Length; i++)
        {
            var obj = Instantiate(prefab);
            icons[i] = obj.GetComponent<Image>();
            obj.transform.SetParent(this.transform);
            obj.transform.localScale = Vector3.one;
            if (inventory.GetSlot(i).item != null)
            {
                icons[i].sprite = inventory.GetSlot(i).item.icon;
            }
            else
            {
                icons[i].sprite = defaultSprite;
            }
        }
    }

    /// <summary>
    /// 인벤토리 열람 시 Image Object별로 배치될 위치를 설정하고 배열에 저장한다.
    /// </summary>
    private void InitSlotPositions()
    {
        if(_slotPositions == null)
            _slotPositions = new Vector3[inventory.Count];

        for (int i = 0, end = _slotPositions.Length; i<end; i++)
        {
            // 원점+원주 상 위치로 설정.
            _slotPositions[i] = Camera.main.ViewportToScreenPoint(Vector3.one* 0.5f);
            _slotPositions[i] += new Vector3(
                            Mathf.Sin(Mathf.PI* 2f * (float) i / end), 
                            Mathf.Cos(Mathf.PI* 2f * (float) i / end)
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
        return (Mathf.RoundToInt(((angle + offset) / 360f) * (inventory.Count))) % (inventory.Count);
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
        for (int i = 0; i < icons.Length; i++)
        {
            if (i == selectedIdx)
            {
                icons[i].transform.localScale = Vector3.one * 1.2f; // 선택된 슬롯 크기 1.2배 키움.
            }
            else
            {
                icons[i].transform.localScale = Vector3.one;
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
        isInventoryOpened = isOpened;
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(isOpened);
        }
    }

    // Fade inout Methods
    /// <summary>
    /// 슬롯 위치를 원점(화면 중앙)에 갖다 놓는다.
    /// </summary>
    private void SetSlotPositionToZeroPoint()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].rectTransform.position = icons[i].rectTransform.position = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);
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
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].rectTransform.position = FadeInLerp(icons[i], _slotPositions[i]);
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

            if(Time.time - time > 5f)   // 연출 코루틴 정지 코드
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

        while (true)
        {
            selectedIndex = GetSelectionIndex();    // 선택된 슬롯 인덱스 설정
            HighlightSelection(selectedIndex);      // 선택된 슬롯 하이라이트
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
