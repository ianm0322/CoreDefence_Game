using System.Collections.Generic;
using Unity.VisualScripting;

namespace BT
{
    public interface INode
    {
        public BTState Evaluate();
    }
}