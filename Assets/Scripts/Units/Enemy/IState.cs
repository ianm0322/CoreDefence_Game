public interface IState
{
    void OnLogicUpdate();
    void OnPhysicsUpdate();
    void OnStateEnter(IState state);
    void OnStateExit(IState state);
}
