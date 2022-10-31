using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BT;
using UnityEngine;

public class Cover : Node
{
    private EnemyRobotBT ebt;

    public Cover(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        if (!ebt.ai.closestCoverPoint)
        {
            float minDistance = 10000;
            foreach (var coverPoint in GameObject.FindGameObjectsWithTag("CoverPoint"))
            {
                float dPointToAI = Vector3.Distance(coverPoint.transform.position, ebt.ai.transform.position);
                float dEnemyToPoint = ebt.ai.enemyObject ? Vector3.Distance(ebt.ai.enemyObject.transform.position, coverPoint.transform.position) : 1;

                bool isBlock = !ebt.ai.enemyObject || Physics.Linecast(coverPoint.transform.position, ebt.ai.enemyObject.transform.position, 1 << LayerMask.NameToLayer("Obstacle"));

                float value = ebt.ai.enemyObject ? (1 / dEnemyToPoint) : dPointToAI;
                if (value < minDistance && isBlock)
                {
                    ebt.ai.closestCoverPoint = coverPoint;
                    minDistance = value;
                }
            }
        }

        if (!ebt.ai.closestCoverPoint) return NodeState.FAILURE;

        ebt.ai.StartMove(ebt.ai.closestCoverPoint.transform.position);

        if (Vector3.Distance(ebt.ai.navAgent.transform.position, ebt.ai.closestCoverPoint.transform.position) <= ebt.ai.navAgent.stoppingDistance)
        {
            Quaternion toRotation = ebt.ai.closestCoverPoint.transform.rotation;
            ebt.ai.transform.rotation = Quaternion.RotateTowards(ebt.ai.transform.rotation, toRotation, Time.deltaTime * 500);

            ebt.ai.inputHandler.isCrouch = true;
        }

        return NodeState.RUNNING;
    }
}
