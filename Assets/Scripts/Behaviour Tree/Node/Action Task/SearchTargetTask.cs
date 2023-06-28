using BT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTargetTask : MonoNode
{
    private EntitySelector _scanner;
    private string _key;
    private Action<Transform> _targetSetter;

    public SearchTargetTask(string key, GameObject gameObject, EntitySelector scanner, Action<Transform> targetSetter) : base(gameObject)
    {
        this._scanner = scanner;
        this._key = key;
        this._targetSetter = targetSetter;
    }

    protected override BTState OnUpdate()
    {
        Transform target = _scanner.ScanEntity();

        _targetSetter?.Invoke(target);  // 결과값 반환

        if (target == null)
        {
            return BTState.Failure;
        }
        else
        {
            return BTState.Success;
        }
    }
}
