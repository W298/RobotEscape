using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyRobotAIState
{
    public class AttackIdleState : AttackBaseState
    {
        public override void Start(StateMachine stateMachine)
        {
            stateMachine.ai.inputHandler.isAim = false;
            stateMachine.ai.inputHandler.isFire = false;
            stateMachine.ai.navAgent.speed = stateMachine.ai.inputHandler.maxSpeed;
        }

        public override void Update(StateMachine stateMachine)
        {

        }
    }

    public class AttackAimState : AttackBaseState
    {
        public AttackAimState(Vector3 attackPosition) => this.attackPosition = attackPosition;
        public AttackAimState(GameObject attackTargetObject) => this.attackTargetObject = attackTargetObject;
        public override void Start(StateMachine stateMachine)
        {
            stateMachine.ai.inputHandler.isAim = true;
            stateMachine.ai.inputHandler.isFire = false;
            stateMachine.ai.navAgent.speed = stateMachine.ai.inputHandler.maxSpeed / 2;
        }

        public override void Update(StateMachine stateMachine)
        {

        }
    }

    public class AttackFireState : AttackBaseState
    {
        public AttackFireState(Vector3 attackPosition) => this.attackPosition = attackPosition;
        public AttackFireState(GameObject attackTargetObject) => this.attackTargetObject = attackTargetObject;
        public override void Start(StateMachine stateMachine)
        {
            stateMachine.ai.inputHandler.isAim = true;
            stateMachine.ai.inputHandler.isFire = true;
            stateMachine.ai.navAgent.speed = stateMachine.ai.inputHandler.maxSpeed / 2;
        }

        public override void Update(StateMachine stateMachine)
        {

        }
    }
}
