using UnityEngine;

public class EntityClassifier_Custom : EntityClassifier
{
    public System.Func<Transform, float> getPriority;
    public System.Func<Transform, bool> filter;

    public EntityClassifier_Custom(Transform origin, string[] tags, System.Func<Transform, float> getPriority = null, System.Func<Transform, bool> filter = null) : base(origin, tags)
    {
        this.getPriority = getPriority;
        this.filter = filter;
    }

    protected override float GetPriority(Transform obj)
    {
        return getPriority(obj);
    }

    protected override bool Filter(Transform obj)
    {
        if (filter == null)
            return base.Filter(obj);
        return base.Filter(obj) && filter(obj);
    }
}