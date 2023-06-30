using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private float _maintenanceTimeLimit = 180f;             // ���� ������ ���ѽð�
    public float MaintenanceTimeLimit => _maintenanceTimeLimit;

    private List<PhaseEvent> _eventQueue = new List<PhaseEvent>();
    public bool EventQueueIsEmpty => _eventQueue.Count == 0;

    public EnemyData TEMP_MINION_DATA;
    public EnemyData TEMP_ROBOT_DATA;

    private void Start()
    {
        // ������ ���� �߰�
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