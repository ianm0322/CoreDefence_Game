using UnityEngine;

public interface IPriorityCalculator
{
    IPriorityCalculator SetNext(IPriorityCalculator next);
    float Calculate(Collider target);
}
