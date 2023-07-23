using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTargetSelector : AbstractTargetSelector
{
    string[] _tags = new string[] { "Player", "Facility", "Core" };

    MinionAI _self;

    public MinionTargetSelector(MinionAI self)
    {
        _self = self;
    }

    protected override IClassifier MakeClassifier()
    {
        return new TagCompareClassifier(_tags)
            .SetNext(new TargetableEntityClassifier())
            .SetNext(new AgentPossibleClassifier(_self.Agent));
    }

    protected override IPriorityCalculator MakePriorityCalculator()
    {
        return new NearestFirstPriorityCalc(_self.transform, _self.AIInfo.DetectRange);
    }

    protected override IScanner MakeScanner()
    {
        return new OverlabSphereScanner(
            origin: _self.transform, 
            range: _self.AIInfo.DetectRange, 
            layer: _self.AIInfo.DetectTargetLayer
            );
    }
}
