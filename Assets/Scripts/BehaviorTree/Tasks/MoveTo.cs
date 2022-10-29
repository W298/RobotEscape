using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : Node
{
    protected Vector3 targetPosition;
    private EnemyRobotBT ebt;

    public MoveTo(Vector3 targetPosition, BehaviorTree bt) : base(bt)
    {
        this.targetPosition = targetPosition;
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        if (Vector3.Distance(ebt.ai.navAgent.transform.position, targetPosition) <= ebt.ai.navAgent.stoppingDistance)
        {
            return NodeState.SUCCESS;
        }

        bool success = ebt.ai.navAgent.SetDestination(targetPosition);
        ebt.ai.navAgent.isStopped = !success;

        return success ? NodeState.RUNNING : NodeState.FAILURE;
    }
}
