using UnityEngine;

namespace BT.DebugNodes
{
    public abstract class DebugNode : BTNode
    {
        public DebugNode() : base()
        {
            Debug.LogWarning("DEBUG LOG EXISTS");
        }
    }

    public abstract class DebugDecoratorNode : DebugNode, IDecoratorNode
    {
        public BTNode content { get; protected set; }

        public DebugDecoratorNode() { }

        public DebugDecoratorNode(BTNode content) : base()
        {
            this.content = content;
            content.Parent = this;
        }

        public void SetContent(BTNode node)
        {
            content = node;
            content.Parent = this;
        }
    }

    public abstract class DebugExecutionNode : BTNode
    {
        public DebugExecutionNode() : base()
        {
        }
    }
}