using System.Collections.Generic;

namespace BT
{
    /// <summary>
    /// [Composite Node] 자식 노드가 성공하면 다음 노드를 실행.
    /// 자식의 결과가 실패하면 진행을 정지하고 실패를 반환한다.
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        private int _index;

        public SequenceNode(params BTNode[] nodes) : base(nodes) { }

        protected override void OnEnter()
        {
            base.OnEnter();
            _index = 0;
        }

        protected override BTState OnUpdate()
        {
            while (_index > childList.Count)
            {
                _index++;

                switch (childList[_index].Evaluate())
                {
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Success:
                        continue;
                    case BTState.Failure:
                        return BTState.Failure;
                }
            }
            return BTState.Success;
        }
    }
}