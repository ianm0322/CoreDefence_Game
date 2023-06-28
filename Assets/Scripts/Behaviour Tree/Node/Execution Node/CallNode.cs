using System;
namespace BT
{
    /// <summary>
    /// (Task) 메서드를 실행한다. 결과에 상관 없이 성공을 반환한다.
    /// </summary>
    public class CallNode : ExecutionNode
    {
        public Action action { get; protected set; }

        public CallNode(Action action)
        {
            this.action = action;
        }

        protected override BTState OnUpdate()
        {
            action?.Invoke();
            return BTState.Success;
        }

        public static implicit operator CallNode(Action action) => new CallNode(action);        // Action과 Action Node를 묵시적으로 변환하여 호환시킨다.
        public static implicit operator Action(CallNode node) => new CallNode(node.action);
    }
}