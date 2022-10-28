using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TakeDistance : Node
{
    private EnemyRobotBT ebt;

    public TakeDistance(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        Vector3 direction = ebt.ai.enemyObject.transform.position - ebt.ai.transform.position;
        float distance = direction.magnitude;

        direction.Normalize();

        switch (distance)
        {
            case < 7.5f:
                ebt.ai.StartMove(ebt.ai.transform.position - direction * 5);
                break;
            case > 10:
                ebt.ai.StartMove(ebt.ai.transform.position + direction * 5);
                break;
            default:
                ebt.ai.StopMove();
                break;
        }

        return NodeState.SUCCESS;
    }
}
