using UnityEngine;

public abstract class EntityClassifier : ClassifierModule<Transform>
{
    public Transform origin;
    public string[] tags;

    public EntityClassifier(Transform origin, string[] tags)
    {
        this.origin = origin;
        this.tags = tags;
    }

    private bool CompareTags(Transform col)
    {
        if(tags == null || tags.Length == 0)    // 분류 지정 테그가 없으면 모든 테그에 대해서 허용.
        {
            return true;
        }

        for (int i = 0; i < tags.Length; i++)   // 타겟의 테그가 분류 지정 테그 목록에 있다면 허용, 없다면 제외
        {
            if (col.CompareTag(tags[i]))
                return true;
        }
        return false;
    }

    protected override bool Filter(Transform obj)
    {
        return CompareTags(obj);   // 목록에 없는 테그를 가진 오브젝트 필터링
    }
} // public abstract class EntityClassifier : Classifier<Collider>
