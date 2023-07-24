using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>, IInitializeOnLoad
{
    [SerializeField]
    InventoryUIScript _inventoryUI;
    public InventoryUIScript InventoryUI => _inventoryUI;

    public void Init()
    {
        FindInventoryUI();
    }

    private void FindInventoryUI()
    {
        if (_inventoryUI == null)
        {
            var invUi = GameObject.Find("InventoryUI");
            if (invUi != null)
            {
                if (invUi.TryGetComponent(out _inventoryUI))
                {
                    // �κ��丮 ui script�� �߰����� ��
                    // set
                }
                else
                {
                    // �κ��丮 ui�� �κ��丮 ui script�� �پ����� ���� ��
                    // �κ��丮 ui�� ��ũ��Ʈ ���̰� �ʱ�ȭ
                }
            }
#if UNITY_EDITOR
            else
            {
                // �κ��丮 ui script�� �߰����� ������ ��
                // �׳� ���� ��ȯ����
                Debug.LogError("UIManager can't find 'InventoryUI' object.");
            }
#endif
        }
    }
}
