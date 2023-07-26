using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WavePhase
{
    public abstract WavePhaseKind Phase { get; protected set; }
    public bool IsStop { get; protected set; } = false;       // true = 웨이브 진행을 멈춤
    public float StartTime;                                     // 웨이브가 시작한 시간. 웨이브 시작시 초기화됨.
    public float ElapsedTime => Time.time - StartTime;

    //protected WaveManager waveManager;

    public WavePhase()
    {
    }

    public virtual void OnUpdate()
    {
    }
    public virtual void OnPhaseStart(WavePhase prePhase)
    {
        StartTime = Time.time;
    }
    public virtual void OnPhaseEnd(WavePhase nextPhase) { }
}
