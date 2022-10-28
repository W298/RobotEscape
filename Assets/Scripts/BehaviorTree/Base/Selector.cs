using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() {}
        public Selector(List<Node> children) : base(children) {}

        public override NodeState Evaluate()
        {

            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.FAILURE:
                        continue;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}
