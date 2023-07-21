using UnityEngine;

public interface ITargetSelector
{
    Collider Find();
    bool Evaluate(Collider col);
}
