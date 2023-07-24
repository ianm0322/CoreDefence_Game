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
        // 1. Scanner로 대상을 탐색하고
        // 2. Classifier로 대상 외 개체를 선별하고
        // 3. PriorityCalculator의 우선순위 계산값을 토대로 가장 우선순위가 높은 개체를 선별
        // 위 일련의 프로세스를 실행하여 적절한 타겟을 구하는 메서드.

        // Scanner 실행: 검사할 충돌체 구하기
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

            // Classifier 실행: 제외 대상 필터링
            if (_classifier.Evaluate(col) == false)
                continue;

            // PriorityCalculator 실행: 우선순위 연산 및 최고우선순위 선별
            float priority = _priorityCalc.Calculate(col);
            if (higherPriority < priority)
            {
                higherPriority = priority;
                higherCollider = col;
            }
        }

        // 결과 반환(결과가 없으면 higherCollider의 초기값 null이 반환됨)
        return higherCollider;
    }

    public bool Evaluate(Collider col)
    {
        return _classifier.Evaluate(col);
    }
}
