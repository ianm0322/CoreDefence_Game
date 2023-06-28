using System.Collections;
using System.Collections.Generic;

public class FacilitiesState_Base : BaseState
{
    public FacilitiesController Self;

    public FacilitiesState_Base(string type, StateMachine machine) : base(type, machine)
    {
        Self = machine as FacilitiesController;
    }
}
