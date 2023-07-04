namespace BT
{
    public class RootNode : BTNode
    {
        public BehaviorTree BT;
        public int Id;
        BTNode node;

        public RootNode(BehaviorTree bt, BTNode node)
        {
            this.node = node;
            BT = bt;
            this.Id = bt.instanceId;
            Attach(node);
        }

        protected override BTState OnUpdate()
        {
            return node.Evaluate();
        }
    }
}