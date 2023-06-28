using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateType = CTType.PlayerControllerStateType;

public class PlayerStateMachine : AbstractStateMachine<StateType, PlayerController>
{
    public PlayerStateMachine(PlayerController controller, StateType defaultType = StateType.OnGround) : base(controller, defaultType)
    {
        stateDict = new Dictionary<StateType, IStateA>()
        {
            {StateType.OnGround, new PlayerState_Move().SetPlayer(this) },
            {StateType.InAir, new PlayerState_InAir().SetPlayer(this) },
            {StateType.Jump, new PlayerState_Jump().SetPlayer(this) },
        };
        InitState(defaultType);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
