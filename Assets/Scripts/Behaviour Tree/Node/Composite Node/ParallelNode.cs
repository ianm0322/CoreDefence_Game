namespace BT
{
    /// <summary>
    /// [Composite Node] 하위 노드를 결과에 상관 없이 모두 실행시키는 노드
    /// </summary>
    public class ParallelNode : CompositeNode
    {
        private int _index;

        public ParallelNode(params BTNode[] nodes) : base(nodes) { }

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
                    case BTState.Running:   // running: 트리 탈출
                        return BTState.Running;
                    case BTState.Success:   // succ/fail: 다음 노드 실행
                    case BTState.Failure:
                        continue;
                }
            }

            return BTState.Success; // 전부 끝났으면 성공 반환
        }
    }
}