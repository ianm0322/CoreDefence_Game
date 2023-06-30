using UnityEngine;

public class PlayerState_Move : PlayerState
{
    Vector3 dir;

    Vector3 look;

    public override void OnStateEnter(IStateA preState)
    {

    }

    public override void OnStateExit(IStateA nextState)
    {

    }

    public override void OnUpdate()
    {
        MoveUpdate();
        JumpUpdate();
    }

    public override void OnFixedUpdate()
    {
        body.Move(dir * player.moveSpeed);
    }

    protected void MoveUpdate()
    {
        // ###### Test: Input은 나중에 따로 분리하기
        float mx = Input.GetAxisRaw("Mouse X") * StaticDataManager.Instance.MouseSensitive;
        float my = Input.GetAxisRaw("Mouse Y") * StaticDataManager.Instance.MouseSensitive;
        look.Set(-my, mx, 0f);
        body.LookDirection += look;

        float h, v;
        h = Input.GetAxisRaw("Horizontal");     // 인풋
        v = Input.GetAxisRaw("Vertical");

        dir = player.transform.forward * v + player.transform.right * h;
        if (h != 0f && v != 0f)
            dir /= 1.4f;
    }

    protected void JumpUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        {
            machine.MoveState(CTType.PlayerControllerStateType.Jump);
        }
        else if (!player._playerMovement.IsGround)
        {
            machine.MoveState(CTType.PlayerControllerStateType.InAir);
        }
    }
}

public class PlayerState_InAir : PlayerState_Move
{
    public override void OnStateEnter(IStateA preState)
    {

    }

    public override void OnUpdate()
    {
        MoveUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        CheckLandingUpdate();
    }

    protected void CheckLandingUpdate()
    {
        if (player._playerMovement.IsGround)
        {
            machine.MoveState(CTType.PlayerControllerStateType.OnGround);
        }
    }
}

public class PlayerState_Jump : PlayerState_Move
{
    float jump_time;

    public override void OnStateEnter(IStateA preState)
    {
        player._playerMovement.UseGravity = false;
        player._playerMovement.DontUpdateVelocity = true;
        jump_time = 0f;
    }

    public override void OnStateExit(IStateA nextState)
    {
        player._playerMovement.UseGravity = true;
        player._playerMovement.DontUpdateVelocity = false;
    }

    public override void OnUpdate()
    {
        MoveUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        CheckFallingUpdate();

        jump_time += Time.fixedDeltaTime;
        body.Jump(player.jumpPower);
        if (jump_time < 1f)
            machine.MoveState(CTType.PlayerControllerStateType.InAir);
    }

    protected void CheckFallingUpdate()
    {
        if (player._playerMovement.IsFalling)   // ############# Last(6.4) state.ground -> jump -> fall로 변환하는 루틴 만들기
        {
            machine.MoveState(CTType.PlayerControllerStateType.InAir);
        }
    }
}

//public class PlayerControllerState_Stun : PlayerControllerState
//{

//}

//public class PlayerControllerState_Die : PlayerControllerState
//{

//}

//public class PlayerControllerState_Interaction : PlayerControllerState
//{

//}