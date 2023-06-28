using System;

namespace BT
{
    public class IndexSelectNode : CompositeNode
    {
        public Func<int> getIndex { get; protected set; }
        protected int _index = 0;

        public IndexSelectNode(Func<int> getIndex, BTNode[] nodes) : base(nodes)
        {
            this.getIndex = getIndex;
        }

        protected override BTState OnUpdate()
        {
            if (State != BTState.Running)   // 이전 결과가 Running이었으면, 이전에 접근했던 인덱스로 접근함.
                _index = getIndex();

            if (_index < childList.Count)
            {
                switch (childList[_index].Evaluate())
                {
                    case BTState.Running:
                        return BTState.Running;
                    case BTState.Success:
                        return BTState.Success;
                    case BTState.Failure:
                        return BTState.Success;
                    case BTState.Abort:
                        return BTState.Failure;
                }
            }
            return BTState.Failure;
        }
    }
}