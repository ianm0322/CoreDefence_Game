using System;
using UnityEngine;

[Serializable]
public class AIData
{
    [Header("AI")]
    public LayerMask DetectTargetLayer;
    public string[] DetectTargetTags;
    /// <summary>
    /// AI가 적을 공격할 수 있다 판단하는 거리.
    /// </summary>
    public float AttackTargetRange = 10f;
    public float TargetMissingRange = 10f; // 타겟이 된 대상의 타겟팅을 해제하는 거리
    public float TargetMissingDelay = 1f;  // 적이 사라졌을 때 적대 상태를 유지하는 시간
    public float DetectRange = 10f;      // 적을 최초 감지하는 거리
    public float DetectDelay = 1f;


    public AIData() { }
    public AIData(AIData data)
    {
        DetectTargetLayer = data.DetectTargetLayer;
        AttackTargetRange = data.AttackTargetRange;
        TargetMissingRange = data.TargetMissingRange;
        TargetMissingDelay = data.TargetMissingDelay;
        DetectRange = data.DetectRange;
        DetectDelay = data.DetectDelay;
    }
}
