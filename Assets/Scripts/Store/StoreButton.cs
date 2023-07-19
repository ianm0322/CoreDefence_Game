using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButton : ButtonBox
{
    [SerializeField]
    private Renderer _childImageRenderer;
    [SerializeField]
    private ItemObjectInfo _product;

    [Header("Options")]
    [SerializeField]
    private bool _isLimittedProduct;
    [SerializeField]
    private int _amount;
    public bool IsSoldOut => (_isLimittedProduct && _amount <= 0);    // ���ѵ� ��ǰ�̰� ������ 0���� ������ ǰ��.

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
        InventoryManager.Instance.AddItem(_product);
        if (_isLimittedProduct)
        {
            _amount--;
        }
        if(IsSoldOut)
        {
            SoldOut();
        }
    }

    private void SoldOut()
    {
        LockButton();
        _childImageRenderer.material.color = Color.red;
    }
}
