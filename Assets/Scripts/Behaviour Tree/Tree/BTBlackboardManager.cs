using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTBlackboardManager<T>
{
    private static BTBlackboardManager<T> _instance;
    public static BTBlackboardManager<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BTBlackboardManager<T>();
            }
            return _instance;
        }
    }

    private Dictionary<int, Dictionary<string, T>> _blackboard;

    public BTBlackboardManager()
    {
        _blackboard = new Dictionary<int, Dictionary<string, T>>();
    }

    #region ��ü ����
    /// <summary>
    /// ��ü�� �ش� �����带 �Ҵ�ް� �ִ��� ���� ��ȯ.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsInstanceRegisted(int id)
    {
        return _blackboard.ContainsKey(id);
    }

    /// <summary>
    /// ��ü�� ������ �Ҵ�
    /// </summary>
    /// <param name="id"></param>
    public void RegistInstance(int id)
    {
        _blackboard.Add(id, new Dictionary<string, T>());
    }

    /// <summary>
    /// ��ü�� �Ҵ�� ������ ����
    /// </summary>
    /// <param name="id">��ü ID</param>
    /// <returns></returns>
    public bool RemoveInstance(int id)
    {
        if (_blackboard.ContainsKey(id) == false)
        {
            return false;
        }
        else
        {
            _blackboard[id].Clear();
            _blackboard.Remove(id);
            return true;
        }
    }
    #endregion ��ü ����

    #region ������ ����
    /// <summary>
    /// Set data in blackboard
    /// </summary>
    /// <param name="id">instance id</param>
    /// <param name="key">data key</param>
    /// <param name="data">new data</param>
    public void SetData(int id, string key, T data)
    {
        _blackboard[id][key] = data;
    }

    /// <summary>
    /// Get data in blackboard.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public T GetData(int id, string key)
    {
        if (_blackboard.ContainsKey(id) == false || _blackboard[id].ContainsKey(key) == false)
        {
            return default(T);
        }
        else
        {
            return _blackboard[id][key];
        }
    }

    public bool RemoveData(int id, string key)
    {
        if (_blackboard.ContainsKey(id) == false || _blackboard[id].ContainsKey(key) == false)  // ��� item�� ������ ���� ����
        {
            return false;
        }
        else
        {
            _blackboard[id].Remove(key);        // ��ü�� key item ����
            return true;
        }
    }

    public bool HasData(int id, string key)
    {
        if(_blackboard.ContainsKey(id) == true && _blackboard[id].ContainsKey(key) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion ������ ����
}
