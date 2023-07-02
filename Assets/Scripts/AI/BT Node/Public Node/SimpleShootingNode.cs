using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class SimpleTurretShootingNode : BTNode
{
    private IShooter _shooter;
    private FacilityData _data;

    private float _timer;

    public SimpleTurretShootingNode(IShooter shooter, FacilityData data)
    {
        this._shooter = shooter;
        this._data = data;
    }

    protected override BTState OnUpdate()
    {
        _timer += Time.deltaTime * _data.AttackSpeed;
        if (_timer > 1f)
        {
            _shooter.Shot();
        }
        return BTState.Success;
    }
}