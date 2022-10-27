using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyRobotAIState
{
    public abstract class BaseState
    {
        public abstract void Start(StateMachine stateMachine);
        public abstract void Update(StateMachine stateMachine);
    }

    public abstract class MovementBaseState : BaseState
    {

    }

    public abstract class AttackBaseState : BaseState
    {
        public Vector3 attackPosition;
        public GameObject attackTargetObject = null;
    }
}
