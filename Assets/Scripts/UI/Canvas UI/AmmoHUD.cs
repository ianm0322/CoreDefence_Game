using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoHUD : MonoBehaviour
{
    public TMP_Text MaxAmmoText;
    public TMP_Text NowAmmoText;

    PlayerController player;

    private void Start()
    {
        player = StageManager.Instance.Player;
    }

    private void LateUpdate()
    {
        if(player._weapon != null)
        {
            SetTextEnabled(true);
            TextUpdate();
        }
        else
        {
            SetTextEnabled(false);
        }
    }

    private void TextUpdate()
    {
        int nowCount = Mathf.Min(player._weapon.AmmoCount, 999);
        int maxCount = Mathf.Min(player._weapon.Data.MaxAmmoAmount, 999);

        NowAmmoText.text = nowCount.ToString();
        MaxAmmoText.text = maxCount.ToString();
    }

    private void SetTextEnabled(bool enabled)
    {
        NowAmmoText.enabled = enabled;
        MaxAmmoText.enabled = enabled;
    }
}
