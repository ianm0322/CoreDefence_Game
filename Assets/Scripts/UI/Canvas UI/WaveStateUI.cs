using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class WaveStateUI : MonoBehaviour
{
    public TMP_Text text;

    StringBuilder sb = new StringBuilder();

    private void Awake()
    {
    }

    void Update()
    {
        sb.Clear();
        if (WaveManager.Instance?.CurrentPhase != null)
        {
            sb.Append("[").Append(WaveManager.Instance.CurrentPhase.Phase.ToString()).Append("]\nTime: ");
            if (WaveManager.Instance.CurrentPhase.Phase == WavePhaseKind.BattlePhase)
                sb.Append($"{WaveManager.Instance.CurrentPhase.ElapsedTime: 0.0}");
            else
                sb.Append($"{WaveManager.Instance.MaintenanceTimeLimit - WaveManager.Instance.CurrentPhase.ElapsedTime: 0.0}");
            text.text = sb.ToString();
        }
    }
}
