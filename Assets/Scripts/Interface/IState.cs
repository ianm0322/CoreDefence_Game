using UnityEngine;

public interface IStateA
{
    void OnUpdate();
    void OnFixedUpdate();
    void OnStateEnter(IStateA preState);
    void OnStateExit(IStateA nextState);
}