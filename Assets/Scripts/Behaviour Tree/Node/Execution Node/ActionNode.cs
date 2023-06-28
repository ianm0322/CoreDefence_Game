using System;
namespace BT
{

    public class ActionNode : ExecutionNode
    {
        public Func<BTState> action { get; protected set; }

        public ActionNode(Func<BTState> action)
        {
            this.action = action;
        }

        protected override BTState OnUpdate()
        {
            return action.Invoke();
        }

        public static implicit operator ActionNode(Func<BTState> action) => new ActionNode(action);        // Action과 Action Node를 묵시적으로 변환하여 호환시킨다.
        public static implicit operator Func<BTState>(ActionNode node) => new ActionNode(node.action);
    }
}