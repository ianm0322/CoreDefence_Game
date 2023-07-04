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

    #region 객체 관리
    /// <summary>
    /// 객체가 해당 블랙보드를 할당받고 있는지 여부 반환.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsInstanceRegisted(int id)
    {
        return _blackboard.ContainsKey(id);
    }

    /// <summary>
    /// 객체에 블랙보드 할당
    /// </summary>
    /// <param name="id"></param>
    public void RegistInstance(int id)
    {
        _blackboard.Add(id, new Dictionary<string, T>());
    }

    /// <summary>
    /// 객체에 할당된 블랙보드 해제
    /// </summary>
    /// <param name="id">객체 ID</param>
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
    #endregion 객체 관리

    #region 데이터 관리
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
        if (_blackboard.ContainsKey(id) == false || _blackboard[id].ContainsKey(key) == false)  // 대상 item이 없으면 삭제 실패
        {
            return false;
        }
        else
        {
            _blackboard[id].Remove(key);        // 객체의 key item 제거
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
    #endregion 데이터 관리
}
