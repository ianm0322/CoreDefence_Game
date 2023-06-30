using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Wave Manager : 인 게임 로직을 담당하는 매니저
 * 배틀 씬을 불러올 때부터 종료할 때까지, 웨이브의 실행, 초기화, 상태 전환, 기록 등을 담당.
 * 
 * 1) 정비시간: 적이 생성되지 않음. 시간제한이 주어지며, 시간제한이 끝나거나 'Input:Key-F'를 3초 이상 누르면 강제 종료된다.
 *      > 시간제한 = 정비시간은 시간제한이 존재하며, 시간제한이 끝나면 State를 전환한다.
 * 2) 전투시간: 적이 정해진 패턴에 따라 생성됨. 시간제한은 없지만 타이머가 돌아가며, 모든 적 등장&파괴 시 종료됨.
 * 3) 게임 오버: 성채 HP<=0일시 게임오버. 모든 적&플레이어의 움직임이 멈추며 점수합산을 보여줌.
 */

public sealed class WaveManager : MonoSingleton<WaveManager>
{
    private Dictionary<WavePhaseKind, WavePhase> _phaseDict;

    public WavePhaseKind DefaultPhase;
    public WavePhaseKind NowPhase => CurrentPhase.Phase;    // 웨이브 상태
    public WavePhase CurrentPhase;

    [field: SerializeField]
    public int WaveLevel { get; set; } = 1;         // 웨이브 레벨

    [SerializeField]
    private float _maintenanceTimeLimit = 180f;             // 정비 페이즈 제한시간
    public float MaintenanceTimeLimit => _maintenanceTimeLimit;

    private List<PhaseEvent> _eventQueue = new List<PhaseEvent>();
    public bool EventQueueIsEmpty => _eventQueue.Count == 0;

    public EnemyData TEMP_MINION_DATA;
    public EnemyData TEMP_ROBOT_DATA;

    private void Start()
    {
        // 페이즈 상태 추가
        _phaseDict = new Dictionary<WavePhaseKind, WavePhase> {
            { WavePhaseKind.MaintenancePhase, new WavePhase_Maintenance() },
            { WavePhaseKind.BattlePhase, new WavePhase_Battle() },
        };
        StartCoroutine(WaveLogicCoroutine());
    }

    private IEnumerator WaveLogicCoroutine()
    {
        yield return null;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        SetPhase(DefaultPhase);

        while (true)
        {
            EventUpdate();
            CurrentPhase.OnUpdate();
            yield return wait;
        }
    }

    private void EventUpdate()
    {
        while (_eventQueue.Count > 0 && CurrentPhase.ElapsedTime > _eventQueue[0].StartTime)
        {
            _eventQueue[0].Execute();
            _eventQueue.RemoveAt(0);
        }
    }

    public void AddEvent(PhaseEvent phaseEvent)
    {
        _eventQueue.Add(phaseEvent);
        _eventQueue.Sort();
    }

    public void SetPhase(WavePhaseKind phase)
    {
        if (CurrentPhase == null)
        {
            CurrentPhase = _phaseDict[phase];
            CurrentPhase.OnPhaseStart(null);
            return;
        }

        WavePhase newPhase = _phaseDict[phase];
        CurrentPhase.OnPhaseEnd(newPhase);
        newPhase.OnPhaseStart(CurrentPhase);
        CurrentPhase = newPhase;
    }
#if UNITY_EDITOR
    //private void OnGUI()
    //{
    //    GUI.TextArea(new Rect(0, 0, 200, 40), $"{NowPhase.ToString()}\n{CurrentPhase.ElapsedTime:0.00}");
    //}
#endif
}