using System;

[Serializable]
public abstract class BaseState : IState
{
    [field: UnityEngine.SerializeField]
    public string StateType { get; private set; }
    public readonly StateMachine machine;

    public BaseState(string type, StateMachine machine)
    {
        this.StateType = type;
        this.machine = machine;
    }

    public virtual void OnLogicUpdate() { }
    public virtual void OnPhysicsUpdate() { }
    public virtual void OnStateEnter(IState state) { }
    public virtual void OnStateExit(IState state) { }
}