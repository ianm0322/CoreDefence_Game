using UnityEngine;

public interface IClassifier
{
    IClassifier SetNext(IClassifier next);
    bool Evaluate(Collider target);
}
