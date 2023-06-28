using UnityEngine;

public class FacilitiesState_Idle : FacilitiesState_Base
{
    public FacilitiesState_Idle(StateMachine machine) : base("Idle", machine)
    {

    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        if (Self.SearchTarget())
        {
            Self.MoveState("Combat");
        }
    }
}

public class FacilitiesState_Search : FacilitiesState_Base
{
    public FacilitiesState_Search(StateMachine machine) : base("Search", machine)
    {

    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        if (Self.SearchTarget())
        {
            Self.MoveState("Combat");
        }
    }
}
