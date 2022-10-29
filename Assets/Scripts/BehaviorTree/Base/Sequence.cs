using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class Sequence : Node
    {
        public Sequence() : base() {}
        public Sequence(List<Node> children) : base(children) {}

        public override NodeState Evaluate()
        {
            bool anyRunning = false;

            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        anyRunning = true;
                        continue;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                }
            }

            state = anyRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}
