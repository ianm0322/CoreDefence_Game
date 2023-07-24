using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct PairElement<TEnum, TContent> where TEnum : System.Enum
    {
        [SerializeField]
        public TEnum key;
        [SerializeField]
        public TContent content;

        public bool TryAddToDictionary(Dictionary<TEnum, TContent> dict)
        {
            return dict.TryAdd(key, content);
        }
    }

    [System.Serializable]
    public struct EnemyDataNode
    {

        [SerializeField]
        public CTType.EnemyKind type;
        [SerializeField]
        public EnemyData content;
    }
}

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

    private List<PhaseEvent> _eventQueueList = new List<PhaseEvent>();
    public bool EventQueueIsEmpty => _eventQueueList.Count == 0;

    [SerializeField]
    private List<Data.PairElement<CTType.EnemyKind, EnemyData>> _enemyBaseDatas;    // 인스펙터에서 받는 베이스 데이터
    private Dictionary<CTType.EnemyKind, EnemyData> _enemyBaseDatasDict;            // 적 객체의 기본 데이터

    #region [Logic]
    protected override void Awake()
    {
        base.Awake();

        // 페이즈 상태 추가
        _phaseDict = new Dictionary<WavePhaseKind, WavePhase> {
            { WavePhaseKind.MaintenancePhase, new WavePhase_Maintenance() },
            { WavePhaseKind.BattlePhase, new WavePhase_Battle() },
        };

        InitDatas();
    }

    private void Start()
    {
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

    // 시간에 따른 이벤트 실행 메소드
    private void EventUpdate()
    {
        // eventqueue의 0번째 데이터는 가장 실행순서가 빠른 이벤트.
        // 0번째 데이터가 실행 대상이 아닐 때까지(시작시간이 현재 진행시간을 넘지 않은 경우) 반복해서 이벤트 실행.
        while (_eventQueueList.Count > 0 && CurrentPhase.ElapsedTime > _eventQueueList[0].StartTime)
        {
            _eventQueueList[0].Execute();
            _eventQueueList.RemoveAt(0);
        }
    }
    #endregion

    #region [Wave Control Method]
    public void StartBattlePhase()
    {
        WaveLevel++;
        SetPhase(WavePhaseKind.BattlePhase);
    }

    public void StartMaintenancePhase()
    {
        SetPhase(WavePhaseKind.BattlePhase);
    }

    public void StartGameOverPhase()
    {
        SetPhase(WavePhaseKind.GameOver);
    }

    /// <summary>
    /// 웨이브를 일시정지시킨다.
    /// </summary>
    public void PauseWave()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 일시정지된 웨이브를 재개시킨다.
    /// </summary>
    public void PlayWave()
    {
        Time.timeScale = 1f;
    }

    #endregion

    #region [Basic Method]
    public void AddEvent(PhaseEvent phaseEvent)
    {
        if (_eventQueueList == null)
        {
            _eventQueueList = new List<PhaseEvent>();
        }

        // 이벤트가 비었으면 추가하고 메서드 종료.
        if (_eventQueueList.Count == 0)
        {
            _eventQueueList.Add(phaseEvent);
            return;
        }

        #region [code description]
        // 이분탐색 알고리즘을 이용해 삽입할 위치를 찾아 삽입=>정렬 상태를 유지하며 event를 삽입한다.
        // 조건: eventqueue는 startTime 기준 오름차순 정렬(더 큰 수=더 나중에 발생)
        // 정의:
        //      low: 탐색할 배열의 첫번째 위치
        //      high: 탐색할 배열의 마지막 위치
        //      mid: low와 high의 중간값. 중간값이 소수일 경우 내림(when low < high, then mid < high)

        // 1. mid = low+high/2
        // 2. repeat while low <= high
        //    if value > mid.value then low = mid+1
        //    if value < mid.value then high = mid-1
        //    if value == mid.value then escape iteration   # value가 mid의 value와 같다면 정지하고 그냥 그 자리에 event를 넣어도 메서드를 종료해도 무관함.
        // # 위 결과 이후 low는 event가 삽입되야 할 위치를 가리킴
        // 3. event_queue.insert(low, event)
        #endregion

        int mid, low, high;
        float mid_time, param_time;
        param_time = phaseEvent.StartTime;
        low = 0;
        high = _eventQueueList.Count - 1;

        while (low <= high)
        {
            mid = (low + high) / 2;
            mid_time = _eventQueueList[mid].StartTime;
            if (param_time > mid_time)
            {
                low = mid + 1;
            }
            else if (param_time < mid_time)
            {
                high = mid - 1;
            }
            else // (param_time == mid_time)
            {
                _eventQueueList.Insert(mid, phaseEvent);
                return;
            }
        }

        // low가 가리키는 삽입 위치가 list의 마지막 위치면 마지막 위치에 add
        if (low == _eventQueueList.Count)
        {
            _eventQueueList.Add(phaseEvent);
        }
        // 아니면 low 위치에 insert
        else
        {
            _eventQueueList.Insert(low, phaseEvent);
        }

        //_eventQueueList.Add(phaseEvent);
        //_eventQueueList.Sort();
    }

    public void SetPhase(WavePhaseKind phase)
    {
        // 이전 페이즈가 없으면(최초 실행시) 새 페이즈만 초기화하고 메서드 종료
        if (CurrentPhase == null)
        {
            CurrentPhase = _phaseDict[phase];
            CurrentPhase.OnPhaseStart(null);
        }
        else
        {
            WavePhase newPhase = _phaseDict[phase];
            CurrentPhase.OnPhaseEnd(newPhase);          // 이전 페이즈 종료시 메서드 실행
            newPhase.OnPhaseStart(CurrentPhase);        // 새 페이즈 시작시 메서드 실행
            CurrentPhase = newPhase;                    // CurrentPhase 변수 업데이트
        }
    }
    #endregion
    public EnemyData GetEnemyData(CTType.EnemyKind enemyKind)
    {
        EnemyData data;
        if (_enemyBaseDatasDict.TryGetValue(enemyKind, out data))
        {
            data = GetUpgradeEnemySpec(data, 1.1f, WaveLevel-1);  // 레벨에 따른 스팩 상승(수정 필요)
            return data;
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogError("Dictionary haz no this element");
            return null;
        }
#endif
    }

    private void InitDatas()
    {
        if (_enemyBaseDatasDict == null)
        {
            _enemyBaseDatasDict = new Dictionary<CTType.EnemyKind, EnemyData>();
        }
        else
        {
            _enemyBaseDatasDict.Clear();
        }
        for (int i = 0; i < _enemyBaseDatas.Count; i++)
        {
            var data = _enemyBaseDatas[i];
            _enemyBaseDatasDict.Add(data.key, data.content);
        }
    }

    private EnemyData GetUpgradeEnemySpec(EnemyData data, float updateRate, float level)
    {
        data.AttackDamage = Mathf.FloorToInt(data.AttackDamage * Mathf.Pow(updateRate, level));
        data.Bullet.damage = data.AttackDamage;
        data.MaxHp = Mathf.FloorToInt(data.MaxHp * Mathf.Pow(updateRate, level));
        data.Hp = Mathf.FloorToInt(data.Hp * Mathf.Pow(updateRate, level));
        data.MoveSpeed = Mathf.FloorToInt(data.MoveSpeed * Mathf.Pow(updateRate, level));

        return data;
    }
}