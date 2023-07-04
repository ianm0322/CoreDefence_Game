using BT;
using System.Collections;
using System.Collections.Generic;

public class SimpleShootingNode : BTNode
{
    private IShooter _shooter;

    public SimpleShootingNode(IShooter shooter)
    {
        this._shooter = shooter;
    }

    protected override BTState OnUpdate()
    {
        _shooter.Shot();
        return BTState.Success;
    }
}
