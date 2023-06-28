using System.Collections.Generic;

namespace BT
{
    public abstract class CompositeNode : BTNode, ICompositeNode
    {
        public List<BTNode> childList { get; protected set; }

        public CompositeNode(IEnumerable<BTNode> nodes) : base()
        {
            using (IEnumerator<BTNode> nodeIter = nodes.GetEnumerator())
            {
                childList = new List<BTNode>();
                foreach (BTNode node in childList)
                {
                    Attach(node);
                }
            }
        }

        public CompositeNode Attach(BTNode node)
        {
            childList.Add(node);
            node.Parent = this;
            return this;
        }
    }
}