using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public abstract class BTNode : INode
    {
        private RootNode _root;

        public BTNode Parent { get; set; }
        public RootNode Root
        {
            get
            {
                if (_root == null)
                {
                    _root = GetRootNode();
                    Blackboard = _root.Blackboard;
                }
                return _root;
            }
        }
        public BTBlackboard Blackboard;
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

        protected RootNode GetRootNode()
        {
            BTNode node = this;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node as RootNode;
        }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected abstract BTState OnUpdate();

        protected virtual void Attach(BTNode node)
        {
            if (node != null)
            {
                node.Parent = this;
            }
        }
    }
}