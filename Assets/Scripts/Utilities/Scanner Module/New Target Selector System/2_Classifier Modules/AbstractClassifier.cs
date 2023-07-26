using UnityEngine;

public abstract class AbstractClassifier : IClassifier
{
    protected IClassifier _next;

    // 연결된 다음 구별기 설정
    public IClassifier SetNext(IClassifier next)
    {
        this._next = next;
        return next;
    }

    public bool Evaluate(Collider target)
    {
        if(target == null)
        {
            return false;
        }

        // 평가 결과 false면 결과 반환
        bool result = Check(target);
        if (result == false)
        {
            return false;
        }

        // 현재 평가 기준 통과했다면, 다음 기준 검사.
        if (_next != null)
        {
            return _next.Evaluate(target);
        }
        // 마지막 평가였다면 성공 결과 반환.
        else
        {
            return true;
        }
    }

    // 충돌체가 타겟팅될 수 있는지 검사하는 메서드. 하위 검별기 객체는 이 메서드만 재정의한다.
    protected abstract bool Check(Collider target);
}
