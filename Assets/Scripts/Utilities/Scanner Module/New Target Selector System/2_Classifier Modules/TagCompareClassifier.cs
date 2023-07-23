using UnityEngine;

public class TagCompareClassifier : AbstractClassifier
{
    private string[] tags;

    public TagCompareClassifier(string[] tags)
    {
        if(tags == null)
        {
            throw new System.Exception("매개변수 tags가 null값을 가지고 있습니다.");
        }
        this.tags = tags;
    }

    protected override bool Check(Collider target)
    {
        return CompareTags(target);
    }

    private bool CompareTags(Collider col)
    {
        // 분류 지정 테그가 없으면 모든 태그에 대해서 허용.
        if (tags.Length == 0)
        {
            return true;
        }

        // 타겟의 태그가 분류 지정 태그 목록에 있다면 허용, 없다면 제외
        for (int i = 0; i < tags.Length; i++)
        {
            if (col.CompareTag(tags[i]) == true)
            {
                return true;
            }
        }

        // 목록중의 타겟이 없으면 
        return false;
    }
}
