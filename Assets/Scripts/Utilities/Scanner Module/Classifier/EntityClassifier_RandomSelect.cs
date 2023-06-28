using UnityEngine;

public class EntityClassifier_RandomSelect : EntityClassifier_Custom
{
    public EntityClassifier_RandomSelect(Transform origin, string[] tags) : base(origin, tags)
    {
    }

    protected override float GetPriority(Transform obj)
    {
        return Random.Range(float.MinValue, float.MaxValue);
    }
} // public class RandomSelectClassifier : EntityClassifier
