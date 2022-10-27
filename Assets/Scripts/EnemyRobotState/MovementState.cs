using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyRobotAIState
{
    public class MovementStopState : MovementBaseState
    {
        public override void Start(StateMachine stateMachine)
        {
            stateMachine.ai.navAgent.isStopped = true;
        }

        public override void Update(StateMachine stateMachine)
        {

        }
    }

    public class MovementMoveState : MovementBaseState
    {
        public Vector3 targetPosition;
        public MovementMoveState(Vector3 targetPosition) => this.targetPosition = targetPosition;

        public override void Start(StateMachine stateMachine)
        {
            Debug.Log("Start Move");
            stateMachine.ai.navAgent.isStopped = false;
            stateMachine.ai.navAgent.SetDestination(targetPosition);
        }

        public override void Update(StateMachine stateMachine)
        {

        }
    }

    public class MovementFollowState : MovementBaseState
    {
        public GameObject targetObject;
        public MovementFollowState(GameObject targetObject) => this.targetObject = targetObject;

        public override void Start(StateMachine stateMachine)
        {
            Debug.Log("Start Following");
            stateMachine.ai.navAgent.isStopped = false;
        }

        public override void Update(StateMachine stateMachine)
        {
            stateMachine.ai.navAgent.SetDestination(targetObject.transform.position);
        }
    }
}
