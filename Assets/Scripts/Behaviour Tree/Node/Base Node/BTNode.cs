using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public abstract class BTNode : INode
    {
        public BTNode Parent { get; set; }
        public BTState State { get; protected set; } = BTState.Success;

        public BTNode()
        {
            Parent = null;
            //Debug.Log(this.GetType());
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

        protected RootNode GetRootNode()
        {
            BTNode node = this;
            while(node.Parent != null)
            {
                node = node.Parent;
            }
            return node as RootNode;
        }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected abstract BTState OnUpdate();
    }
}