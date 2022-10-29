using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        public NodeState state;

        protected BehaviorTree bt;
        protected List<Node> children;

        public Node()
        {
            this.bt = null;
            this.children = new List<Node>();
        }

        public Node(BehaviorTree bt)
        {
            this.bt = bt;
            this.children = new List<Node>();
        }

        public Node(List<Node> children)
        {
            this.bt = null;
            this.children = children;
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;
    }
}
