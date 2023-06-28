using System;
using System.Collections.Generic;

namespace BT
{
    public interface INodeVisitor
    {
        void Record(INode node);
        INode GetLastNode();
    }
}