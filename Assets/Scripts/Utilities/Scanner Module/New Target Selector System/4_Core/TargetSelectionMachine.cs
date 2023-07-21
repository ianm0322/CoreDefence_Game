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
        // �˻��� �浹ü ���ϱ�
        Collider[] cols = scanner.FindColliders();
        Collider col;
        Collider higherCollider = null;
        float higherPriority = float.NegativeInfinity;

        for (int i = 0; i < cols.Length; i++)
        {
            col = cols[i];

            // �˻翡�� ������ ������ ��� ���͸�
            if (classifier.Evaluate(col) == false)
                continue;

            // �켱���� ���� �� �켱������ ���� ����
            float priority = priorityCalc.Calculate(col);
            if (higherPriority < priority)
            {
                higherPriority = priority;
                higherCollider = col;
            }
        }

        // ��� ��ȯ(����� ������ null ��ȯ)
        return higherCollider;
    }

    public bool Evaluate(Collider col)
    {
        return classifier.Evaluate(col);
    }
}
