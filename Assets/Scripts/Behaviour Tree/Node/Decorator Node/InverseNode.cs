using System;
namespace BT
{
    /// <summary>
    /// 하위 노드의 실행결과를 반전시킴.
    /// </summary>
    public class InverseNode : DecoratorNode
    {
        public InverseNode(BTNode content) : base(content)
        {
        }

        protected override BTState OnUpdate()
        {
            switch (content.Evaluate())
            {
                case BTState.Running:
                    return BTState.Running;
                case BTState.Success:
                    return BTState.Failure;
                case BTState.Failure:
                    return BTState.Success;
            }
            return BTState.Failure;
        }
    }
}
