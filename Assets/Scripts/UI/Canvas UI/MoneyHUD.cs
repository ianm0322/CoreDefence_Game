using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class MoneyHUD : MonoBehaviour
{
    public TMP_Text MoneyText;
    [Multiline]
    [Tooltip("[0] = money")]
    public string format;

    StringBuilder sb = new StringBuilder();

    private void LateUpdate()
    {
        TextUpdate();
    }

    private void TextUpdate()
    {
        int money = StageManager.Instance.Money;

        sb.AppendFormat(format, money);
        MoneyText.text = sb.ToString();
        sb.Clear();
    }

    private void SetTextEnabled(bool enabled)
    {
        MoneyText.enabled = enabled;
    }
}
