using BT;
using UnityEngine;

public class SimpleTurretShootingNode : BTNode
{
    private IShooter _shooter;
    private FacilityData _data;

    private float _rapidTimer;
    private float _cooldownTimer;
    private bool _isCooldown = false;
    private int _count;

    public SimpleTurretShootingNode(IShooter shooter, FacilityData data)
    {
        this._shooter = shooter;
        this._data = data;
    }

    protected override BTState OnUpdate()
    {
        if (_isCooldown)
        {
            CooldownState();
            return BTState.Failure;
        }
        else
        {
            FireState();
            return BTState.Success;
        }
    }

    private void CooldownState()
    {
        _cooldownTimer += Time.deltaTime;
        if(_cooldownTimer > _data.AttackDelay)
        {
            _isCooldown = false;
            _cooldownTimer = 0;
        }
    }

    private void FireState()
    {
        _rapidTimer += Time.deltaTime * _data.AttackSpeed;
        if (_rapidTimer > 1f)
        {
            _shooter.Shot();
            _rapidTimer = 0;
            if (++_count >= _data.AttackCount)
            {
                _count = 0;
                _isCooldown = true;
            }
        }
    }
}