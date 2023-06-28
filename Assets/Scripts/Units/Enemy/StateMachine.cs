using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineEventListenerArgs : EventArgs
{
    public BaseState preState;
    public BaseState newState;
}

public abstract class StateMachine : MonoBehaviour
{
    protected Dictionary<string, BaseState> stateDict = new Dictionary<string, BaseState>();

    [field: SerializeField]
    public BaseState CurrentState { get; private set; }
    public string DefaultState;

    public event EventHandler StateMachineEvent;
    private StateMachineEventListenerArgs _smEventArgs = new StateMachineEventListenerArgs();

    private void Start()
    {
        MoveState(DefaultState);
    }

    protected virtual void Update()
    {
        OnLogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        OnPhysicsUpdate();
    }

    public void OnLogicUpdate()
    {
        CurrentState.OnLogicUpdate();
    }

    public void OnPhysicsUpdate()
    {
        CurrentState.OnPhysicsUpdate();
    }

    protected virtual void OnMoveState(BaseState preState, BaseState newState)
    {
        _smEventArgs.preState = preState;
        _smEventArgs.newState = newState;
        StateMachineEvent?.Invoke(this, _smEventArgs);
    }

    public void MoveState(BaseState state)
    {
        BaseState preState = CurrentState;
        if (CurrentState == null)
        {
            CurrentState = state;
            CurrentState.OnStateEnter(null);
        }
        else
        {
            CurrentState.OnStateExit(state);    // ���� ������Ʈ ������ �̺�Ʈ ����
            state.OnStateEnter(CurrentState);   // ���� ������Ʈ �� �̺�Ʈ ����
            CurrentState = state;               // ���� ���� �̵�
        }
        OnMoveState(preState, state);
    }

    public void MoveState(string state)
    {
        if (stateDict.ContainsKey(state) == false)
        {
            throw new System.Exception($"{name} ������Ʈ�� ���¸ӽſ� {state} ���´� �����ϴ�.");
        }

        MoveState(stateDict[state]);
    }

    protected void AddState(BaseState state)
    {
        stateDict.Add(state.StateType, state);
    }

    protected virtual void OnDestroy()
    {
        stateDict.Clear();
    }
}
