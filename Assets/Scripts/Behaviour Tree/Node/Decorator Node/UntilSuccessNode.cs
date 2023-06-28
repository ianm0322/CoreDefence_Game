namespace BT
{
    /// <summary>
    /// 노드가 성공할 때까지 반복
    /// </summary>
    public class UntilSuccessNode : DecoratorNode
    {
        public UntilSuccessNode(BTNode content) : base(content)
        {
        }

        protected override BTState OnUpdate()
        {
            BTState result = content.Evaluate();
            switch (result)
            {
                case BTState.Running:
                    return BTState.Running;
                case BTState.Success:
                    return BTState.Success;
                case BTState.Failure:       // 노드가 실패하면 다음 프레임에 다시 반복
                    return BTState.Running;
                default:
                    break;
            }
            return BTState.Failure;
        }
    }
}