using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public interface IBehaviorTree
    {
        RootNode MakeBT();
        void Operate();
    }
}