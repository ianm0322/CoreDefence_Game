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
        // 1. Scanner�� ����� Ž���ϰ�
        // 2. Classifier�� ��� �� ��ü�� �����ϰ�
        // 3. PriorityCalculator�� �켱���� ��갪�� ���� ���� �켱������ ���� ��ü�� ����
        // �� �Ϸ��� ���μ����� �����Ͽ� ������ Ÿ���� ���ϴ� �޼���.

        // Scanner ����: �˻��� �浹ü ���ϱ�
        int count = _scanner.FindColliders(_foundColliders);
        if (count == 0)
        {
            return null;
        }
        Collider col;   // ���� �˻��� Ÿ��
        Collider higherCollider = null; // �ְ���� Ÿ��
        float higherPriority = float.NegativeInfinity;  // �ְ���� �켱����

        for (int i = 0; i < count; i++)
        {
            col = _foundColliders[i];

            // Classifier ����: ���� ��� ���͸�
            if (_classifier.Evaluate(col) == false)
                continue;

            // PriorityCalculator ����: �켱���� ���� �� �ְ�켱���� ����
            float priority = _priorityCalc.Calculate(col);
            if (higherPriority < priority)
            {
                higherPriority = priority;
                higherCollider = col;
            }
        }

        // ��� ��ȯ(����� ������ higherCollider�� �ʱⰪ null�� ��ȯ��)
        return higherCollider;
    }

    public bool Evaluate(Collider col)
    {
        return _classifier.Evaluate(col);
    }
}
