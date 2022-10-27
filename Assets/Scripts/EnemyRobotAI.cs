using System.Collections;
using System.Collections.Generic;
using EnemyRobotAIState;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRobotAI : MonoBehaviour
{
    public RobotInputHandler inputHandler;
    public NavMeshAgent navAgent;

    public GameObject exTarget;

    public MovementStateMachine mStateMachine;
    public AttackStateMachine aStateMachine;

    public void AIReloadNeed()
    {
        inputHandler.Reload();
    }

    private void Start()
    {
        inputHandler = GetComponent<RobotInputHandler>();
        navAgent = GetComponent<NavMeshAgent>();

        navAgent.updateRotation = false;

        mStateMachine = new MovementStateMachine(this);
        aStateMachine = new AttackStateMachine(this);

        mStateMachine.Start();
        aStateMachine.Start();
    }

    private void FixedUpdate()
    {
        mStateMachine.Update();
        aStateMachine.Update();
    }
}
