using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public abstract class BehaviorTree : MonoBehaviour
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
