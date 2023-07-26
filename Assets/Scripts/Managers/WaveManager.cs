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

/* Wave Manager : �� ���� ������ ����ϴ� �Ŵ���
 * ��Ʋ ���� �ҷ��� ������ ������ ������, ���̺��� ����, �ʱ�ȭ, ���� ��ȯ, ��� ���� ���.
 * 
 * 1) ����ð�: ���� �������� ����. �ð������� �־�����, �ð������� �����ų� 'Input:Key-F'�� 3�� �̻� ������ ���� ����ȴ�.
 *      > �ð����� = ����ð��� �ð������� �����ϸ�, �ð������� ������ State�� ��ȯ�Ѵ�.
 * 2) �����ð�: ���� ������ ���Ͽ� ���� ������. �ð������� ������ Ÿ�̸Ӱ� ���ư���, ��� �� ����&�ı� �� �����.
 * 3) ���� ����: ��ä HP<=0�Ͻ� ���ӿ���. ��� ��&�÷��̾��� �������� ���߸� �����ջ��� ������.
 */
public sealed class WaveManager : MonoSingleton<WaveManager>
{
    private Dictionary<WavePhaseKind, WavePhase> _phaseDict;

    public WavePhaseKind DefaultPhase;
    public WavePhaseKind NowPhase => CurrentPhase.Phase;    // ���̺� ����
    public WavePhase CurrentPhase;

    [field: SerializeField]
    public int WaveLevel { get; set; } = 1;         // ���̺� ����
    private float _enemyPowerRate = 1f;

    [SerializeField]
    private float _maintenanceTimeLimit = 180f;             // ���� ������ ���ѽð�
    public float MaintenanceTimeLimit => _maintenanceTimeLimit;
    private float _gameStartTime;
    public float GameStartTime => _gameStartTime;
    private float _gameEndTime;

    private List<PhaseEvent> _eventQueueList = new List<PhaseEvent>();
    public bool EventQueueIsEmpty => _eventQueueList.Count == 0;

    [SerializeField]
    private List<Data.PairElement<CTType.EnemyKind, EnemyData>> _enemyBaseDatas;    // �ν����Ϳ��� �޴� ���̽� ������
    private Dictionary<CTType.EnemyKind, EnemyData> _enemyBaseDatasDict;            // �� ��ü�� �⺻ ������

    #region [Logic]
    protected override void Awake()
    {
        base.Awake();

    }

    public override void InitOnSceneLoad(string sceneName)
    {
        if (sceneName == "PlayScene")
        {
            Init();
            StartWave();
            MyDebug.Log("WaveManager is initialized!");
        }
    }

    public void Init()
    {
        InitCollections();
        InitPhase();
        //InitManagerData();
    }

    public void StartWave()
    {
        InitGameData();
        StartCoroutine(WaveLogicCoroutine());
        StartMaintenancePhase();
    }

    public void EndGame()
    {
        StopAllCoroutines();
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

    // �ð��� ���� �̺�Ʈ ���� �޼ҵ�
    private void EventUpdate()
    {
        // eventqueue�� 0��° �����ʹ� ���� ��������� ���� �̺�Ʈ.
        // 0��° �����Ͱ� ���� ����� �ƴ� ������(���۽ð��� ���� ����ð��� ���� ���� ���) �ݺ��ؼ� �̺�Ʈ ����.
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
        SetPhase(WavePhaseKind.BattlePhase);
    }

    public void StartMaintenancePhase()
    {
        WaveLevel++;
        SetPhase(WavePhaseKind.MaintenancePhase);
    }

    public void StartGameOverPhase()
    {
        SetPhase(WavePhaseKind.GameOver);
    }

    /// <summary>
    /// ���̺긦 �Ͻ�������Ų��.
    /// </summary>
    public void PauseWave()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// �Ͻ������� ���̺긦 �簳��Ų��.
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

        // �̺�Ʈ�� ������� �߰��ϰ� �޼��� ����.
        if (_eventQueueList.Count == 0)
        {
            _eventQueueList.Add(phaseEvent);
            return;
        }

        #region [code description]
        // �̺�Ž�� �˰����� �̿��� ������ ��ġ�� ã�� ����=>���� ���¸� �����ϸ� event�� �����Ѵ�.
        // ����: eventqueue�� startTime ���� �������� ����(�� ū ��=�� ���߿� �߻�)
        // ����:
        //      low: Ž���� �迭�� ù��° ��ġ
        //      high: Ž���� �迭�� ������ ��ġ
        //      mid: low�� high�� �߰���. �߰����� �Ҽ��� ��� ����(when low < high, then mid < high)

        // 1. mid = low+high/2
        // 2. repeat while low <= high
        //    if value > mid.value then low = mid+1
        //    if value < mid.value then high = mid-1
        //    if value == mid.value then escape iteration   # value�� mid�� value�� ���ٸ� �����ϰ� �׳� �� �ڸ��� event�� �־ �޼��带 �����ص� ������.
        // # �� ��� ���� low�� event�� ���ԵǾ� �� ��ġ�� ����Ŵ
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

        // low�� ����Ű�� ���� ��ġ�� list�� ������ ��ġ�� ������ ��ġ�� add
        if (low == _eventQueueList.Count)
        {
            _eventQueueList.Add(phaseEvent);
        }
        // �ƴϸ� low ��ġ�� insert
        else
        {
            _eventQueueList.Insert(low, phaseEvent);
        }

        //_eventQueueList.Add(phaseEvent);
        //_eventQueueList.Sort();
    }

    public void SetPhase(WavePhaseKind phase)
    {
        // ���� ����� ������(���� �����) �� ����� �ʱ�ȭ�ϰ� �޼��� ����
        MyDebug.Log("Set Phase " + phase.ToString());
        if (CurrentPhase == null)
        {
            CurrentPhase = _phaseDict[phase];
            CurrentPhase.OnPhaseStart(null);
        }
        else
        {
            WavePhase newPhase = _phaseDict[phase];
            CurrentPhase.OnPhaseEnd(newPhase);          // ���� ������ ����� �޼��� ����
            newPhase.OnPhaseStart(CurrentPhase);        // �� ������ ���۽� �޼��� ����
            CurrentPhase = newPhase;                    // CurrentPhase ���� ������Ʈ
        }
    }
    #endregion

    public EnemyData GetEnemyData(CTType.EnemyKind enemyKind)
    {
        EnemyData data;
        if (_enemyBaseDatasDict.TryGetValue(enemyKind, out data))
        {
            data = GetUpgradeEnemySpec(data, _enemyPowerRate);
            return data;
        }
        else
        {
            MyDebug.Log("Dictionary haz no this element");
            return null;
        }
    }

    private void InitCollections()
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

        if (_eventQueueList == null)
        {
            _eventQueueList = new List<PhaseEvent>();
        }
        else
        {
            _eventQueueList.Clear();
        }
    }

    private void InitPhase()
    {
        if (_phaseDict == null)
        {
            _phaseDict = new Dictionary<WavePhaseKind, WavePhase>();
        }
        else
        {
            _phaseDict.Clear();
        }
        // ������ ���� �߰�
        _phaseDict.Add(WavePhaseKind.MaintenancePhase, new WavePhase_Maintenance());   // ���� ������
        _phaseDict.Add(WavePhaseKind.BattlePhase, new WavePhase_Battle());        // ���� ������
    }

    private void InitGameData()
    {
        WaveLevel = 0;
        _gameStartTime = Time.time;
        _enemyPowerRate = 1f;
    }

    private EnemyData GetUpgradeEnemySpec(EnemyData data, float updateRate)
    {
        EnemyData newData = new EnemyData(data);
        newData.AttackDamage = Mathf.FloorToInt(data.AttackDamage * updateRate);
        newData.Bullet.damage = data.AttackDamage;
        newData.MaxHp = Mathf.FloorToInt(data.MaxHp * updateRate);
        newData.Hp = Mathf.FloorToInt(data.Hp * updateRate);
        newData.MoveSpeed = Mathf.FloorToInt(data.MoveSpeed * updateRate);

        return newData;
    }

    public void MultipleEnemyPowerRate(float addition)
    {
        _enemyPowerRate *= addition;
    }

    public void KillAllEnemy()
    {

    }
}