using System.Collections.Generic;

namespace BT
{
    /// <summary>
    /// [Composite Node] ��尡 ������ ������ ���� ��带 ��ȸ.
    /// ��� ������尡 �����ϸ� ���� ��ȯ.
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