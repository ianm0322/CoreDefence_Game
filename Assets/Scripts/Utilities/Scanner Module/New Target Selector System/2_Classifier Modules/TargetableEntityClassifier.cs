using UnityEngine;

public class TargetableEntityClassifier : AbstractClassifier
{
    protected override bool Check(Collider target)
    {
        CD_GameObject obj;

        if (target.TryGetComponent(out obj) == false)
        {
            return false;
        }

        if (obj.CanFocus == false)
        {
            return false;
        }

        return false;
    }
}
