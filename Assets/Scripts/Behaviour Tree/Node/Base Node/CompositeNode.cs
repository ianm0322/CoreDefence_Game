using System.Collections.Generic;

namespace BT
{
    public abstract class CompositeNode : BTNode, ICompositeNode
    {
        public List<BTNode> childList { get; protected set; }


        public CompositeNode() : base()
        {
            childList = new List<BTNode>();
        }

        public CompositeNode(IEnumerable<BTNode> nodes) : base()
        {
            childList = new List<BTNode>();
            if (nodes != null)
            {
                using (IEnumerator<BTNode> nodeIter = nodes.GetEnumerator())
                {
                    foreach (BTNode node in nodes)
                    {
                        Attach(node);
                    }
                }
            }
        }

        public CompositeNode(params BTNode[] nodes) : base()
        {
            childList = new List<BTNode>();
            if (nodes != null)
            {
                foreach (BTNode node in nodes)
                {
                    Attach(node);
                }
            }
        }

        public void Attach(BTNode node)
        {
            if (node != null)
            {
                childList.Add(node);
                node.Parent = this;
            }
        }
    }
}