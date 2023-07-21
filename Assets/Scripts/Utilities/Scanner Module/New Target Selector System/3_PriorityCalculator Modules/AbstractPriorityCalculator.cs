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
        // 자신의 보정값 계산
        float result = GetPriority(target);

        // 연결된 계산기가 없으면 결과 반환
        if (_next != null)
        {
            return result;
        }
        // 다음 평가 항목이 있다면 계산 후 현재 값에 더함.
        else
        {
            return result + _next.Calculate(target);
        }
    }

    /// <summary>
    /// <br>우선순위 보정값을 반환한다.</br>
    /// <br>우선순위 계산기의 모든 보정값이 더해져 최종 결과로 합산된다.</br>
    /// <br>반환값의 크기는 0..1을 표준으로 한다.</br>
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected abstract float GetPriority(Collider target);
}
