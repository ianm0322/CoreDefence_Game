namespace BT
{
    public class RootNode : BTNode
    {
        BTNode node;

        public RootNode(BTNode node)
        {
            this.node = node;
        }

        protected override BTState OnUpdate()
        {
            return node.Evaluate();
        }
    }
}