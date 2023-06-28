namespace BT
{
    public abstract class DecoratorNode : BTNode, IDecoratorNode
    {
        public BTNode content { get; protected set; }

        public DecoratorNode(BTNode content) : base()
        {
            this.content = content;
            if(content != null)
                content.Parent = this;
        }
    }
}