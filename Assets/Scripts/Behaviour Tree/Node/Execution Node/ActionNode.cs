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

        public static implicit operator ActionNode(Func<BTState> action) => new ActionNode(action);        // Action�� Action Node�� ���������� ��ȯ�Ͽ� ȣȯ��Ų��.
        public static implicit operator Func<BTState>(ActionNode node) => new ActionNode(node.action);
    }
}