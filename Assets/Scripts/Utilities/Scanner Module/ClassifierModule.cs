public abstract class ClassifierModule<T> where T : class
{
    public virtual T GetObject(T[] objects, int length)
    {
        if (objects.Length == 0)
            return null;
        else
        {
            if (length > objects.Length)
                length = objects.Length;

            int resultIdx = -1;  // 현재 가장 우선순위가 높은 인덱스를 저장
            float higherPriority = float.NegativeInfinity;  // 가장 높은 우선순위

            T target;

            for (int i = 0; i < length; i++)
            {
                target = objects[i];
                if (!Filter(target))    // 필터 결과값이 false면 대상 제외
                    continue;
                float curPriority = GetPriority(target);    // 타겟의 우선순위를 구한 뒤 우선순위 검사.
                if (higherPriority < curPriority) // 우선순위가 더 높은 대상 발견시 교체
                {
                    resultIdx = i;
                    higherPriority = curPriority;
                }
            }

            if (resultIdx == -1)    // 초기값(-1) 그대로면 대상이 없으므로 null 반환
                return null;
            return objects[resultIdx];
        }
    }
    public virtual T[] SortByPriority(T[] objects, int length)
    {
        throw new System.NotImplementedException();
    }
    public virtual bool Check(T obj)
    {
        return Filter(obj);
    }
    /// <summary>
    /// <br>객체의 우선순위를 반환하는 메서드. </br>
    /// <br>float값이 높을수록 높은 우선순위이며, 대상에서 제외시키고 싶다면 float.NegativeInfinite를 반환한다.</br>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected abstract float GetPriority(T obj);
    /// <summary>
    /// <br>비교 대상군에서 제외시킬 오브젝트 판별 메서드. </br>
    /// <br>재정의 메서드가 false를 반환하면 해당 객체는 비교대상에서 제외된다. </br>
    /// <br>우선순위 계산보다 먼저 실행된다.</br>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected abstract bool Filter(T obj);
}
