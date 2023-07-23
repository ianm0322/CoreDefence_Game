using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TargetSelectorTest : MonoBehaviour
{
    ITargetSelector _selector;

    void Start()
    {
        _selector = new TestSelector(this.gameObject);
    }

    void Update()
    {
        Collider target = _selector.Find();

        if(target != null)
        {
            Debug.Log("find target: " + target.name);
        }
        else
        {
            Debug.Log("target is null");
        }
    }
}

public class TestSelector : AbstractTargetSelector
{
    GameObject self;

    public TestSelector(GameObject self)
    {
        this.self = self;
    }

    protected override IClassifier MakeClassifier()
    {
        return new TagCompareClassifier(new string[] { "Player" })
            .SetNext(new NoObstacleClassifier(self.transform, LayerMask.GetMask("Facility")));
    }

    protected override IPriorityCalculator MakePriorityCalculator()
    {
        return new NearestFirstPriorityCalc(self.transform, 10);
    }

    protected override IScanner MakeScanner()
    {
        return new OverlabSphereScanner(self.transform, 10, LayerMask.GetMask("Enemy"));
    }
}