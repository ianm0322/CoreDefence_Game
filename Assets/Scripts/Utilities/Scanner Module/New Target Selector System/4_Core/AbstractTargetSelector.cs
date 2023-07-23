using UnityEngine;

public abstract class AbstractTargetSelector : ITargetSelector
{
    private TargetSelectionMachine _selector;

    public AbstractTargetSelector()
    {
    }

    public Collider Find()
    {
        if(_selector == null)
        {
            _selector = Produce();
        }

        return _selector.Find();
    }

    public bool Evaluate(Collider col)
    {
        if (_selector == null)
        {
            _selector = Produce();
        }

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
