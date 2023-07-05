using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetNode : ExecutionNode
{
    ITargetter _controller;

    bool _isTargetSetNull;

    public SetTargetNode(ITargetter controller) : base()
    {
        this._controller = controller;
        SetOption(isTargetSetNull: false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isTargetSetNull">true: Ÿ���� �������� ���ϸ� null ����. false: ���� ���� ����</param>
    /// <returns></returns>
    public SetTargetNode SetOption(bool isTargetSetNull = false)
    {
        this._isTargetSetNull = isTargetSetNull;
        return this;
    }

    protected override BTState OnUpdate()
    {
        var tr = _controller.Scanner.ScanEntity();
        if(tr != null)
        {
            tr.GetComponent<CD_GameObject>().FocusCount++;
            _controller.Target = tr;
        }
        else if(_isTargetSetNull == true)
        {
            _controller.Target = null;
        }

        if(_controller.Target == null)
        {
            return BTState.Failure;
        }
        else
        {
            return BTState.Success;
        }
    }


}
