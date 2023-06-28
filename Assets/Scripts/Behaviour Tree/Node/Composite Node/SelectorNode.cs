using System.Collections.Generic;

namespace BT
{
    /// <summary>
    /// [Composite Node] 노드가 성공할 때까지 하위 노드를 순회.
    /// 모든 하위노드가 실패하면 실패 반환.
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        private int _index;

        public SelectorNode(params BTNode[] nodes) : base(nodes) { }


        protected override void OnEnter()
        {
            base.OnEnter();
            _index = 0;
        }

        protected override BTState OnUpdate()
        {
            BTNode node;

            for (int end = childList.Count; _index < end; _index++)
            {
                node = childList[_index];

                switch (node.Evaluate())
                {
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Success:
                        return BTState.Success;
                    case BTState.Failure:
                        continue;
                }
            }

            return BTState.Failure;
        }
    }
}