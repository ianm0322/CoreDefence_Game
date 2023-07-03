using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class CountdownNode : ExecutionNode
{
    int _count = 0;
    int _maxCount;

    bool _isSuccessOverCount = true;

    public CountdownNode(int maxium)
    {
        this._maxCount = maxium;
        SetOption();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isSuccessOverCount">true: 평가가 일정 횟수 도달하면 Success를 반환함.</param>
    /// <returns></returns>
    public CountdownNode SetOption(bool isSuccessOverCount = true)
    {
        _isSuccessOverCount = isSuccessOverCount;
        return this;
    }

    protected override BTState OnUpdate()
    {
        _count++;
        if (_count > _maxCount)  // Over Count
        {
            _count = 0;
            return (_isSuccessOverCount) ? BTState.Success : BTState.Failure;
        }
        else
        {
            return (_isSuccessOverCount) ? BTState.Failure : BTState.Success;
        }

    }
}
