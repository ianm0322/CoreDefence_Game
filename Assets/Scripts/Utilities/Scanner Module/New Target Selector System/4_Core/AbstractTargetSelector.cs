using UnityEngine;

public abstract class AbstractTargetSelector : ITargetSelector
{
    private TargetSelectionMachine _selector;

    public AbstractTargetSelector()
    {
        _selector = Produce();
    }

    public Collider Find()
    {
        return _selector.Find();
    }

    public bool Evaluate(Collider col)
    {
        return _selector.Evaluate(col);
    }

    public TargetSelectionMachine Produce()
    {
        TargetSelectionMachine tempSelector = new TargetSelectionMachine();
        tempSelector.SetScanner(MakeScanner());
        tempSelector.SetClassifier(MakeClassifier());
        tempSelector.SetPriorityCalculator(MakePriorityCalculator());
        return tempSelector;
    }

    protected abstract IClassifier MakeClassifier();
    protected abstract IPriorityCalculator MakePriorityCalculator();
    protected abstract IScanner MakeScanner();
}
