using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTree : MonoBehaviour
    {
        private Node _root = null;

        private void Start()
        {
            _root = CreateTree();
        }

        private void FixedUpdate()
        {
            _root?.Evaluate();
        }

        protected abstract Node CreateTree();
    }
}
