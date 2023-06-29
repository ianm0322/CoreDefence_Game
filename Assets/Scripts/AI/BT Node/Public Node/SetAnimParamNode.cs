using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimParamNode : ExecutionNode
{
    enum AnimParamKind
    {
        SetBool,
        SetInt,
        SetFloat,
        SetTrigger
    }

    AnimParamKind _type;

    string _key;
    Animator _anim;
    bool _boolean;
    float _float;
    int _int;

    public SetAnimParamNode(Animator anim, string key) : base()
    {
        this._anim = anim;
        _key = key;
        _type = AnimParamKind.SetTrigger;
    }

    public SetAnimParamNode(Animator anim, string key, bool b) : this(anim, key)
    {
        this._anim = anim;
        _key = key;
        _type = AnimParamKind.SetBool;
    }

    public SetAnimParamNode(Animator anim, string key, int i)
    {
        this._anim = anim;
        _key = key;
        _type = AnimParamKind.SetTrigger;
    }

    public SetAnimParamNode(Animator anim, string key, float f)
    {
        this._anim = anim;
        _key = key;
        _type = AnimParamKind.SetTrigger;
    }

    protected override BTState OnUpdate()
    {
        switch (_type)
        {
            case AnimParamKind.SetBool:
                _anim.SetBool(_key, _boolean);
                break;
            case AnimParamKind.SetInt:
                _anim.SetInteger(_key, _int);
                break;
            case AnimParamKind.SetFloat:
                _anim.SetFloat(_key, _float);
                break;
            case AnimParamKind.SetTrigger:
                _anim.SetTrigger(_key);
                break;
        }
        return BTState.Success;
    }
}
