using System;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<TStateEnum, TController>
{
    TController Self { get; }
    TStateEnum CurrentState { get; }
    void OnFixedUpdate();
    void OnUpdate();
    void MoveState(TStateEnum state);
    void InitState(TStateEnum state);
}

public class AbstractStateMachine<TStateEnum, TController> : IStateMachine<TStateEnum, TController> where TStateEnum : Enum where TController : MonoBehaviour
{
    protected Dictionary<TStateEnum, IStateA> stateDict;
    public TController Self { get; private set; }
    public TStateEnum CurrentState { get; private set; }
    protected IStateA current => stateDict[CurrentState];

    public AbstractStateMachine(TController controller, TStateEnum defaultType = default(TStateEnum))
    {
        Self = controller;   // 상태에 제어될 대상 컴포넌트
        CurrentState = defaultType;    // 디폴트 스테이트로 상태 설정
    }

    public virtual void OnFixedUpdate()
    {
        current.OnFixedUpdate();
    }

    public virtual void OnUpdate()
    {
        current.OnUpdate();
    }

    /// <summary>
    /// 기존 상태에서 나가고 새 상태로 이동하기.
    /// </summary>
    /// <param name="state"></param>
    public void MoveState(TStateEnum state)
    {
        stateDict[CurrentState].OnStateExit(stateDict[state]);  // 현재 스테이트 나가기 이벤트 실행
        stateDict[state].OnStateEnter(stateDict[CurrentState]); // 현재 스테이트 들어감 이벤트 실행
        CurrentState = state;   // 현재 상태 이동
    }

    /// <summary>
    /// 기존 상태를 무시하고 새 상태로 설정하기.
    /// </summary>
    /// <param name="state"></param>
    public void InitState(TStateEnum state)
    {
        if (!stateDict.ContainsKey(state))
            Debug.Log("IDONTKNOW");
        stateDict[state].OnStateEnter(null);
    }
}
