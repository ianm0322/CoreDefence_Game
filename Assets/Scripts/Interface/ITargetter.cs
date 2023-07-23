using UnityEngine;

public interface ITargetter
{
    Collider GetTarget();
    void SetTarget(Collider target);
    ITargetSelector GetTargetSelector();
}
