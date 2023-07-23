using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class TargetSelectionMachine : ITargetSelector
{
    IScanner _scanner;
    IClassifier _classifier;
    IPriorityCalculator _priorityCalc;

    Collider[] _foundColliders = new Collider[8];

    public void SetScanner(IScanner scanner)
    {
        this._scanner = scanner;
    }
    public void SetClassifier(IClassifier classifier)
    {
        this._classifier = classifier;
    }
    public void SetPriorityCalculator(IPriorityCalculator priorityCalc)
    {
        this._priorityCalc = priorityCalc;
    }

    public Collider Find()
    {
        // 검사할 충돌체 구하기
        int count = _scanner.FindColliders(_foundColliders);
        if (count == 0)
        {
            return null;
        }
        Collider col;   // 현재 검사할 타겟
        Collider higherCollider = null; // 최고순위 타겟
        float higherPriority = float.NegativeInfinity;  // 최고순위 우선순위

        for (int i = 0; i < count; i++)
        {
            col = _foundColliders[i];

            // 검사에서 완전히 제외할 대상 필터링
            if (_classifier.Evaluate(col) == false)
                continue;

            // 우선순위 연산 후 우선순위에 따라 정렬
            float priority = _priorityCalc.Calculate(col);
            if (higherPriority < priority)
            {
                higherPriority = priority;
                higherCollider = col;
            }
        }

        // 결과 반환(결과가 없으면 null 반환)
        return higherCollider;
    }

    public bool Evaluate(Collider col)
    {
        return _classifier.Evaluate(col);
    }
}
