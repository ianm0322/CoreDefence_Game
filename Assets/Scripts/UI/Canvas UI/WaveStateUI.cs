using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class WaveStateUI : MonoBehaviour
{
    public TMP_Text text;
    [Multiline]
    [Tooltip("[0] = current phase\n[1] = wave level\n[2] = time")]
    public string format = "[{0} : Wave {1}]\nTIME: {2:0.00}";

    StringBuilder sb = new StringBuilder();

    private void Awake()
    {
    }

    void Update()
    {
        TextUpdate();
    }

    private void TextUpdate()
    {
        if (WaveManager.Instance?.CurrentPhase != null)
        {
            int level = WaveManager.Instance.WaveLevel;
            WavePhaseKind phase = WaveManager.Instance.CurrentPhase.Phase;
            float time = Mathf.Max(WaveManager.Instance.CurrentPhase.ElapsedTime, 0f);
            if (phase == WavePhaseKind.MaintenancePhase)
            {
                time = WaveManager.Instance.MaintenanceTimeLimit - time;
            }

            // [CurrentPhase : 0 Wave]
            // Time : 00.00
            sb.AppendFormat(format, phase.ToString(), level, time);

            text.text = sb.ToString();
        }
        sb.Clear();
    }
}
