using UnityEngine;

public class EntityClassifier_NearSort : EntityClassifier_Custom
{
    public EntityClassifier_NearSort(Transform origin, string[] tags) : base(origin, tags)
    {
    }

    protected override float GetPriority(Transform obj)
    {
        return -Vector3.Distance(origin.position, obj.position);
    }
} // public class NearSortClassifier : EntityClassifier
