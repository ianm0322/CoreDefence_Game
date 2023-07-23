using System;
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
        float resultPriority = GetPriority(target);

        // 연결된 계산기가 없으면 결과 반환
        if (_next == null)
        {
            return resultPriority;
        }
        // 다음 평가 항목이 있다면 계산 후 현재 값에 곱함.
        // 값을 곱하면 노드별 계산비중 격차 문제가 해소됨.
        // ex) 거리계산->고유 우선순위 계산일 때, 거리계산은 0..100인 반면 고유 우선순위가 0..1이면 후자의 값이 전자에 희석됨.
        // 반면 값을 곱하면 
        // 단, 노드 중 하나라도 0이 나오면 항상 0이 되므로 원치 않는 결과가 나올 수 있음.
        // 이 부분은... 일단 0이 곱해지지 않도록 조심하는 걸로...
        else
        {
            return resultPriority * _next.Calculate(target);
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
