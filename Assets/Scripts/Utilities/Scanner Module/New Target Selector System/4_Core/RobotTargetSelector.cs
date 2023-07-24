using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTargetSelector : AbstractTargetSelector
{
    string[] _tags = new string[] { "Player", "Facility", "Core" };
    LayerMask _obstacleLayer = LayerMask.GetMask("Wall");

    Transform _selfTr;
    AIData _data;

    public RobotTargetSelector(Transform tr, AIData data)
    {
        this._selfTr = tr;
        this._data = data;
    }

    protected override IScanner MakeScanner()
    {
        return new OverlabSphereScanner(_selfTr, _data.DetectRange, _data.DetectTargetLayer);
    }

    protected override IClassifier MakeClassifier()
    {
        return new TagCompareClassifier(_tags)
            .SetNext(new TargetableEntityClassifier())
            .SetNext(new NoObstacleClassifier(_selfTr, _obstacleLayer));
    }

    protected override IPriorityCalculator MakePriorityCalculator()
    {
        return new NearestFirstPriorityCalc(_selfTr, _data.DetectRange);
    }
}
