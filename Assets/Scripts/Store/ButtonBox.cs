using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonBox : MonoBehaviour, IInteractable
{
    private Animator _anim;

    private UnityAction _action;

    public bool IsPressed { get; set; }
    public bool IsLocked { get; set; }

    public bool canPress => !(IsPressed || IsLocked);

    protected virtual void Awake()
    {
        TryGetComponent(out _anim);
    }

    protected virtual void OnInteract() { }

    public bool Interact()
    {
        if (canPress)
        {
            IsPressed = true;
            PressButton();
            OnInteract();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetLock(bool boolean)
    {
        _anim.SetBool("IsLocked", boolean);
    }

    public void Init()
    {
        _anim.SetBool("IsLocked", false);
    }

    private void PressButton()
    {
        _anim.SetBool("IsPressed", true);
        MyDebug.Log("Is Pressed!");
    }

    private void OnPressedEvent()
    {
        _anim.SetBool("IsPressed", true);
    }

    private void OnPressDone()
    {
        _anim.SetBool("IsPressed", false);
    }

    private void OnReleasedEvent()
    {
        IsPressed = false;
    }
}
