namespace BT
{
    public class SuccessNode : DecoratorNode
    {
        public SuccessNode(BTNode content) : base(content)
        {
        }

        protected override BTState OnUpdate()
        {
            var result = content.Evaluate();
            if (result == BTState.Running)
                return BTState.Running;
            return BTState.Success;
        }
    }
}
