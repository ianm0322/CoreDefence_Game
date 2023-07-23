using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetNode : ExecutionNode
{
    ITargetter _self;

    bool _isTargetSetNull;

    public SetTargetNode(ITargetter self) : base()
    {
        this._self = self;
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
        if (_self.GetTargetSelector() == null) Debug.Log("Target Selector is null");
        var target = _self.GetTargetSelector().Find();
        if (target != null)
        {
            // ����� �����Ǹ� ��Ŀ�� �õ�. ��Ŀ�� �����ߴٸ� 
            if (target.GetComponent<CD_GameObject>().AddFocus() == true)
            {
                _self.SetTarget(target);
                return BTState.Success;
            }
            else
            {
                return BTState.Failure;
            }
        }
        else if (_isTargetSetNull == true)
        {
            _self.SetTarget(null);
            return BTState.Failure;
        }
        return BTState.Failure;
    }
}
