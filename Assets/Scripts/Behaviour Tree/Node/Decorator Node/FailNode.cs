namespace BT
{
    public class FailNode : DecoratorNode
    {
        public FailNode(BTNode content) : base(content)
        {
        }

        protected override BTState OnUpdate()
        {
            var result = content.Evaluate();
            if (result == BTState.Running)
                return BTState.Running;
            return BTState.Failure;
        }
    }
}
