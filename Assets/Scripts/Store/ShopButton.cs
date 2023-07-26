using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : ButtonBox
{
    [SerializeField]
    private Renderer _childImageRenderer;
    [Header("Product Properties")]
    [SerializeField]
    private ItemObjectInfo _product;    // �Ǹ� ������
    [SerializeField]
    private int _price; // ����

    [Header("Options")]
    [SerializeField]
    private bool _isLimittedProduct;    // �������� ��ǰ���� ����
    [SerializeField]
    private int _stock;                // ���� ����
    public bool IsSoldOut => (_isLimittedProduct && _stock <= 0);    // ���ѵ� ��ǰ�̰� ������ 0���� ������ ǰ��.

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _childImageRenderer.material.mainTexture = _product.icon.texture;
    }

    protected override void OnInteract()
    {
        if(BuyItem())
        {
            MyDebug.Log("���� ����");

        }
        else
        {
            MyDebug.Log("���� ����");
        }
        if (IsSoldOut)
        {
            SoldOut();
        }
    }

    private bool BuyItem()
    {
        // 1. ���� ���� ����Ѱ�?
        if (HasPlayerEnoughMoney() && HasShopEnoughStock())
        {
            // 2. ������ �߰� �õ�
            bool result = GiveItem();
            if (result)
            {
                // 3. ������ �߰� �����ߴٸ� ���� ���� ���ҽ�Ų��.
                DecreaseStock();
                StageManager.Instance.SubtractMoney(_price);
                return true;
            }
        }
        return false;
    }

    private bool GiveItem()
    {
        return StageManager.Instance.AddItem(_product);
    }

    private bool HasPlayerEnoughMoney()
    {
        return StageManager.Instance.CanSpendMoney( _price);
    }

    private bool HasShopEnoughStock()
    {
        // ������ǰ�� �ƴϰų� ��� ����ϸ� ���� ����.
        return !_isLimittedProduct || _stock > 0;
    }

    private void SoldOut()
    {
        SetLock(true);
        _childImageRenderer.material.color = Color.red;
    }

    private bool DecreaseStock()
    {
        if (_isLimittedProduct && HasShopEnoughStock())
        {
            _stock--;
            return true;
        }
        else
        {
            return false;
        }
    }
}
