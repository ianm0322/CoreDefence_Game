public abstract class PlayerState : IStateA
{
    protected PlayerController player;
    protected PlayerStateMachine machine;
    protected PlayerBody body;

    public PlayerState SetPlayer(PlayerStateMachine machine)
    {
        this.machine = machine;
        this.player = machine.Self;
        this.body = player.PlayerMovement;
        return this;
    }
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnStateEnter(IStateA preState);
    public abstract void OnStateExit(IStateA nextState);
}