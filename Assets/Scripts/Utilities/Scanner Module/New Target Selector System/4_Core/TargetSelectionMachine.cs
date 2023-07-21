using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectionMachine
{
    IScanner scanner;
    IClassifier classifier;
    IPriorityCalculator priorityCalc;

    public void SetScanner(IScanner scanner)
    {
        this.scanner = scanner;
    }
    public void SetClassifier(IClassifier classifier)
    {
        this.classifier = classifier;
    }
    public void SetPriorityCalculator(IPriorityCalculator priorityCalc)
    {
        this.priorityCalc = priorityCalc;
    }

    public Collider Find()
    {
        // 검사할 충돌체 구하기
        Collider[] cols = scanner.FindColliders();
        Collider col;
        Collider higherCollider = null;
        float higherPriority = float.NegativeInfinity;

        for (int i = 0; i < cols.Length; i++)
        {
            col = cols[i];

            // 검사에서 완전히 제외할 대상 필터링
            if (classifier.Evaluate(col) == false)
                continue;

            // 우선순위 연산 후 우선순위에 따라 정렬
            float priority = priorityCalc.Calculate(col);
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
        return classifier.Evaluate(col);
    }
}
