using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoSingleton<UpdateManager>
{
    public List<GameObject> Pool;

    #region Test
    // ���� ������Ʈ ����Ʈ
    private List<IUpdateListener> _updateList;
    private List<ILateUpdateListener> _lateList;
    private List<IFixedUpdateListener> _fixedList;

    // �Ͻ� ������Ʈ ����Ʈ
    private Queue<IUpdateListener> _updateQueue;
    private Queue<ILateUpdateListener> _lateQueue;
    private Queue<IFixedUpdateListener> _fixedQueue;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        InitCollections();
    }

    private void Start()
    {
        foreach (var item in Pool)
        {
            Join(item, UpdateType.Update);
        }
    }

    private void Update()
    {
        // ���� ������Ʈ ����
        for (int i = _updateList.Count - 1, end = 0; i >= end; i--)
        {
            if (_updateList[i] == null)
            {
                _updateList.RemoveAt(i);
            }
            else
            {
                _updateList[i].OnUpdate();
            }
        }

        // ��ȸ ������Ʈ ����
        for (int i = 0, end = _updateQueue.Count; i < end; i++)
        {
            _updateQueue.Dequeue().OnUpdate();
        }
    }

    private void LateUpdate()
    {
        // ���� ������Ʈ ����
        for (int i = _lateList.Count - 1, end = 0; i >= end; i--)
        {
            if (_lateList[i] == null)
            {
                _lateList.RemoveAt(i);
            }
            else
            {
                _lateList[i].OnLateUpdate();
            }
        }

        // ��ȸ ������Ʈ ����
        for (int i = 0, end = _lateQueue.Count; i < end; i++)
        {
            _lateQueue.Dequeue().OnLateUpdate();
        }
    }

    private void FixedUpdate()
    {
        // ���� ������Ʈ ����
        for (int i = _fixedList.Count - 1, end = 0; i >= end; i--)
        {
            if (_fixedList[i] == null)
            {
                _fixedList.RemoveAt(i);
            }
            else
            {
                _fixedList[i].OnFixedUpdate();
            }
        }

        // ��ȸ ������Ʈ ����
        for (int i = 0, end = _fixedQueue.Count; i < end; i++)
        {
            _fixedQueue.Dequeue().OnFixedUpdate();
        }
    }

    private void InitCollections()
    {
        _updateList = new List<IUpdateListener>();
        _lateList = new List<ILateUpdateListener>();
        _fixedList = new List<IFixedUpdateListener>();

        _updateQueue = new Queue<IUpdateListener>();
        _lateQueue = new Queue<ILateUpdateListener>();
        _fixedQueue = new Queue<IFixedUpdateListener>();
    }

    #region Public Method
    #region [Add Listener Method]
    public void Join(IUpdateListener listener)
    {
        _updateList.Add(listener);
    }

    public void Join(ILateUpdateListener listener)
    {
        _lateList.Add(listener);
    }

    public void Join(IFixedUpdateListener listener)
    {
        _fixedList.Add(listener);
    }

    public void Join(GameObject obj, UpdateType type)
    {
        if ((type & UpdateType.Update) != 0)
        {
            _updateList.Add(obj.GetComponent<IUpdateListener>());
        }
        if ((type & UpdateType.LateUpdate) != 0)
        {
            _lateList.Add(obj.GetComponent<ILateUpdateListener>());
        }
        if ((type & UpdateType.FixedUpdate) != 0)
        {
            _fixedList.Add(obj.GetComponent<IFixedUpdateListener>());
        }
    }

    public void AddQueue(IUpdateListener listener)
    {
        _updateQueue.Enqueue(listener);
    }

    public void AddQueue(ILateUpdateListener listener)
    {
        _lateQueue.Enqueue(listener);
    }

    public void AddQueue(IFixedUpdateListener listener)
    {
        _fixedQueue.Enqueue(listener);
    }
    #endregion [Add Listener Method]

    #region [Remove Listener Method]
    /// <summary>
    /// ������ ���� ������Ʈ ������ ������.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    public void Release<T>(T listener, UpdateType type) where T : MonoBehaviour
    {
        if ((type & UpdateType.Update) != 0)
        {
            _updateList.Remove(listener as IUpdateListener);
        }
        if ((type & UpdateType.LateUpdate) != 0)
        {
            _lateList.Remove(listener as ILateUpdateListener);
        }
        if ((type & UpdateType.FixedUpdate) != 0)
        {
            _fixedList.Remove(listener as IFixedUpdateListener);
        }
    }

    /// <summary>
    /// �ش� ������Ʈ�� ��� ���� ������Ʈ ������ ������.
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseAll<T>(T listener) where T : MonoBehaviour
    {
        var updater = listener as IUpdateListener;
        var lateUpdater = listener as ILateUpdateListener;
        var fixedUpdater = listener as IFixedUpdateListener;

        if (updater != null && _updateList.Contains(updater))   // ������Ʈ �����ϰ� ����Ʈ�� ���Ե� ������ ����
        {
            _updateList.Remove(listener as IUpdateListener);
        }
        if (lateUpdater != null && _lateList.Contains(lateUpdater))   // ����Ʈ ������Ʈ �����ϰ� ����Ʈ ����Ʈ�߿� ���Ե� ������ ����
        {
            _lateList.Remove(listener as ILateUpdateListener);
        }
        if (fixedUpdater != null && _fixedList.Contains(fixedUpdater))  // �Ƚ��� ������Ʈ �����ϰ� ����Ʈ ����Ʈ�߿� ���Ե� ������ ����
        {
            _fixedList.Remove(listener as IFixedUpdateListener);
        }
    }

    /// <summary>
    /// ������ �Ͻ� ������Ʈ Ǯ�� ������.
    /// </summary>
    /// <param name="type"></param>
    public void ReleaseQueue(UpdateType type)
    {
        if ((type & UpdateType.Update) != 0)
        {
            _updateQueue.Clear();
        }
        if ((type & UpdateType.LateUpdate) != 0)
        {
            _lateQueue.Clear();
        }
        if ((type & UpdateType.FixedUpdate) != 0)
        {
            _fixedQueue.Clear();
        }
    }

    /// <summary>
    /// ��� ������Ʈ ������ ������.
    /// </summary>
    public void Clear()
    {
        _updateList.Clear();
        _lateList.Clear();
        _fixedList.Clear();

        _updateQueue.Clear();
        _lateQueue.Clear();
        _fixedQueue.Clear();
    }
    #endregion [Remove Listener Method]
    #endregion
}

[System.Flags]
public enum UpdateType : byte
{
    None = 0,
    Update = 1 << 0,
    LateUpdate = 1 << 1,
    FixedUpdate = 1 << 2,
    Everything = 255
}