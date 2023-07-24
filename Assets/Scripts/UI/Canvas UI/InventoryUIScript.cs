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

    [Obsolete("�׽�Ʈ�� �ӽ� �޼���.")]
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
            // ������ ������ ���
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
    //    // �κ��丮�� �������� ��, ���� ����Ű�� �ִ� ������ �����ϴ� �޼���
    //    slot = null;
    //}

    //public void SelectPointedSlot()
    //{
    //    // �κ��丮�� �������� ��, ���� ����Ű�� �ִ� ������ �����ϴ� �޼���
    //}

    //public Image HoldSlot()
    //{
    //    return null;
    //}

    //public void PutSlot()
    //{
    //    // Held slot�� ���� ����Ű�� ��ġ�� �̵���Ų��.
    //}

    /// <summary>
    /// �κ��丮�� ����.
    /// </summary>
    public void OpenInventory()
    {
        StateInitialize(true);
        ExecuteCoroutine(ref _mainCoroutine, InventoryOpenUpdateCor);
        ExecuteCoroutine(ref _effectCoroutine, InventoryFadeInCor);
    }

    /// <summary>
    /// �κ��丮�� �ݴ´�.
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
    /// Slot�� ǥ���� Image Object�� �����ϰ� �迭�� �����Ѵ�.
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
    /// �κ��丮 ���� �� Image Object���� ��ġ�� ��ġ�� �����ϰ� �迭�� �����Ѵ�.
    /// </summary>
    private void InitSlotPositions()
    {
        if (_slotPositions == null)
            _slotPositions = new Vector3[inventory.Count];

        for (int i = 0, end = _slotPositions.Length; i < end; i++)
        {
            // ����+���� �� ��ġ�� ����.
            _slotPositions[i] = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);
            _slotPositions[i] += new Vector3(
                            Mathf.Sin(Mathf.PI * 2f * (float)i / end),
                            Mathf.Cos(Mathf.PI * 2f * (float)i / end)
                            ) * Screen.height / 3;
        }
    }
    #endregion

    /// <summary>
    /// ������ ����Ǵ� �ڷ�ƾ�� �ߴ��ϰ� ���ο� �ڷ�ƾ�� �����Ű�� �޼���.
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
    /// ȭ�� �߾ӿ��� ���콺 ��ġ������ ���͸� ��ȯ�Ѵ�.
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
    /// ������������ ���콺 ��ġ������ ������ ��ȯ�Ѵ�. vector.up = 0��, vector.right = 90��
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
    /// ������ n����Ͽ�, �� �� �� ��° ������ �ִ��� index�� ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private int AngleToIndex(float angle)
    {
        /*
         * [�ؼ�]
         * 1. angle = angle + offset    => ������ ����
         * 2. angle / 360               => angle = 0..1
         * 3. angle * n                 => index = 0~n
         * 4. index %= n                => index�� 0~n-1 ���̰��̹Ƿ�, index==n�϶� 0���� �������.
         */
        return (Mathf.RoundToInt(((angle + Offset) / 360f) * (inventory.Count))) % (inventory.Count);
    }
    /// <summary>
    /// ���콺 ��ġ�� �߾����κ��� ����� ��ġ�� ������ false�� ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="mouse"></param>
    /// <returns></returns>
    private bool CheckSelectionDist(Vector2 mouse)
    {
        return mouse.magnitude > Screen.height / 10;
    }
    /// <summary>
    /// ���콺�� ��ġ�� ���� ���õ� �κ��丮 ������ �ε����� ��ȯ�Ѵ�.
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
    /// ���õ� ������ �����Ѵ�.
    /// </summary>
    /// <param name="selectedIdx"></param>
    private void HighlightSelection(int selectedIdx)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            if (i == selectedIdx)
            {
                _icons[i].transform.localScale = Vector3.one * 1.2f; // ���õ� ���� ũ�� 1.2�� Ű��.
            }
            else
            {
                _icons[i].transform.localScale = Vector3.one;
            }
        }
    }
    /// <summary>
    /// �κ��丮 open/close ���¿� ���� �ʿ��� �ʱ�ȭ�� ���ִ� �޼���.
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
    /// ���� ��ġ�� ����(ȭ�� �߾�)�� ���� ���´�.
    /// </summary>
    private void SetSlotPositionToZeroPoint()
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].rectTransform.position = _icons[i].rectTransform.position = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);
        }
    }
    /// <summary>
    /// ������ ��ġ �������� ��ȯ�Ѵ�.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector3 FadeInLerp(Image target, Vector3 position)
    {
        return Vector3.Lerp(target.rectTransform.position, position, Time.deltaTime * 5f);
    }
    /// <summary>
    /// ���� ��ġ�� �ֽ�ȭ��Ų��.
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
    /// �κ��丮 �� �� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator InventoryFadeInCor()
    {
        float time = Time.time;
        yield return null;

        SetSlotPositionToZeroPoint();   // ���� ��ġ�� ������ ����

        while (true)
        {
            UpdateSlotPositions();      // ���� ��ġ ������Ʈ

            if (Time.time - time > 5f)   // ���� �ڷ�ƾ ���� �ڵ�
            {
                _effectCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }
    /// <summary>
    /// �κ��丮�� �����ִ� ���� �۵��� �ڷ�ƾ
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

            // ������ �ű�� �ڵ�

            // 1. �����ܿ� ���콺 Ŭ�� ��
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

            CursoredIndex = GetSelectionIndex();    // ���õ� ���� �ε��� ����
            HighlightSelection(CursoredIndex);      // ���õ� ���� ���̶���Ʈ

            yield return null;
        }
    }
    /// <summary>
    /// �κ��丮�� ���� ��.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InventoryCloseCor()
    {
        yield return null;
        _mainCoroutine = null;
    }
    #endregion
}
