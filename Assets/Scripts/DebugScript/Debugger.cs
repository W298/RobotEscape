using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using EnemyRobotAIState;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyAI;

    private void Start()
    {
        SROptions.Current.player = player;
        SROptions.Current.enemyAI = enemyAI;
    }
}

public partial class SROptions
{
    public GameObject player;
    public GameObject enemyAI;

    public Vector3 moveDestination = new Vector3(-8.5f, 0, 7.9f);

    [Category("Movement")]
    public void FollowState()
    {
        enemyAI.GetComponent<EnemyRobotAI>().mStateMachine.SwitchState(new MovementFollowState(player));
    }

    [Category("Movement")]
    public void MoveState()
    {
        enemyAI.GetComponent<EnemyRobotAI>().mStateMachine.SwitchState(new MovementMoveState(moveDestination));
    }

    [Category("Movement")]
    public void StopState()
    {
        enemyAI.GetComponent<EnemyRobotAI>().mStateMachine.SwitchState(new MovementStopState());
    }

    [Category("Attack")]
    public void AimState()
    {
        enemyAI.GetComponent<EnemyRobotAI>().aStateMachine.SwitchState(new AttackAimState(player));
    }

    [Category("Attack")]
    public void FireState()
    {
        enemyAI.GetComponent<EnemyRobotAI>().aStateMachine.SwitchState(new AttackFireState(player));
    }

    [Category("Attack")]
    public void IdleState()
    {
        enemyAI.GetComponent<EnemyRobotAI>().aStateMachine.SwitchState(new AttackIdleState());
    }
}
