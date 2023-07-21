using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    public Slider slider;
    public CD_GameObject player;

    private void Start()
    {
        TryGetComponent(out slider);
        StageInfoManager.Instance.Player.TryGetComponent(out player);

        slider.minValue = 0;
        slider.maxValue = 1;
    }

    private void LateUpdate()
    {
        slider.value = (float)player.Hp / (float)player.MaxHp;
    }
}
