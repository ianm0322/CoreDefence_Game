using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPriorityCalculator : IPriorityCalculator
{
    private IPriorityCalculator _next;

    public IPriorityCalculator SetNext(IPriorityCalculator next)
    {
        this._next = next;
        return next;
    }

    public float Calculate(Collider target)
    {
        // �ڽ��� ������ ���
        float result = GetPriority(target);

        // ����� ���Ⱑ ������ ��� ��ȯ
        if (_next != null)
        {
            return result;
        }
        // ���� �� �׸��� �ִٸ� ��� �� ���� ���� ����.
        else
        {
            return result + _next.Calculate(target);
        }
    }

    /// <summary>
    /// <br>�켱���� �������� ��ȯ�Ѵ�.</br>
    /// <br>�켱���� ������ ��� �������� ������ ���� ����� �ջ�ȴ�.</br>
    /// <br>��ȯ���� ũ��� 0..1�� ǥ������ �Ѵ�.</br>
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected abstract float GetPriority(Collider target);
}
