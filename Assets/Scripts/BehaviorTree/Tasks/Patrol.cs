using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class Patrol : Node
{
    private EnemyRobotBT ebt;

    private float patrolWaitDuration = 3;
    private float patrolWaitTimer;

    private int current = 0;
    private int reverse = -1;

    public Patrol(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
        patrolWaitTimer = patrolWaitDuration;
    }

    public override NodeState Evaluate()
    {
        int count = ebt.ai.patrolPoints.transform.childCount;
        Vector3 targetPosition = ebt.ai.patrolPoints.transform.GetChild(current).transform.position;

        float remainDistance = Vector3.Distance(ebt.ai.navAgent.transform.position, targetPosition);
        if (remainDistance <= ebt.ai.navAgent.stoppingDistance)
        {
            patrolWaitTimer -= Time.deltaTime;
            if (patrolWaitTimer < 0)
            {
                patrolWaitTimer += patrolWaitDuration;

                if (current == 0 || current == count - 1) reverse *= -1;
                current += reverse;
            }
        }

        bool success = ebt.ai.StartMove(targetPosition);
        return success ? NodeState.RUNNING : NodeState.FAILURE;
    }
}
