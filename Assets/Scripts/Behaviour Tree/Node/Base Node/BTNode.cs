using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BT
{
    public abstract class BTNode : INode
    {
        public BTNode Parent { get; internal set; }
        public BTState State { get; protected set; } = BTState.Success;

        public BTNode()
        {
            Parent = null;
        }

        public BTState Evaluate()
        {
            if (State != BTState.Running)
            {
                OnEnter();
            }
            State = OnUpdate();
            if (State != BTState.Running)
            {
                OnExit();
            }
            return State;
        }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected abstract BTState OnUpdate();
    }
}