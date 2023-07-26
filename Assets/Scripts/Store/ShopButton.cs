using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : ButtonBox
{
    [SerializeField]
    private Renderer _childImageRenderer;
    [Header("Product Properties")]
    [SerializeField]
    private ItemObjectInfo _product;    // 판매 아이템
    [SerializeField]
    private int _price; // 가격

    [Header("Options")]
    [SerializeField]
    private bool _isLimittedProduct;    // 갯수제한 상품인지 여부
    [SerializeField]
    private int _stock;                // 남은 갯수
    public bool IsSoldOut => (_isLimittedProduct && _stock <= 0);    // 제한된 상품이고 수량이 0보다 낮으면 품절.

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
            MyDebug.Log("구매 성공");

        }
        else
        {
            MyDebug.Log("구매 실패");
        }
        if (IsSoldOut)
        {
            SoldOut();
        }
    }

    private bool BuyItem()
    {
        // 1. 재고와 돈이 충분한가?
        if (HasPlayerEnoughMoney() && HasShopEnoughStock())
        {
            // 2. 아이템 추가 시도
            bool result = GiveItem();
            if (result)
            {
                // 3. 아이템 추가 성공했다면 재고와 돈을 감소시킨다.
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
        // 한정상품이 아니거나 재고가 충분하면 구매 가능.
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
