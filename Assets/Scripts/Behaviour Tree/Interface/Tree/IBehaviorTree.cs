using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public interface IBehaviorTree
    {
        RootNode GenerateBT();
        void Operate();
    }
}