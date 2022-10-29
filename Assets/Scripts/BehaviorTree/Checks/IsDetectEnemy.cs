using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BT;
using UnityEngine;

public class IsDetectEnemy : Node
{
    private EnemyRobotBT ebt;

    public IsDetectEnemy(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        if (!ebt.ai.isHit)
        {
            GameObject enemy = ebt.ai.visonSensor.detectedObjectList.Find(o => o.name == "Player");
            ebt.ai.enemyObject = enemy;
        }

        if (ebt.ai.enemyObject)
        {
            ebt.ai.seekLevel = 100;
            ebt.ai.seekLevelDsc = false;
            ebt.ai.lastEnemyPosition = ebt.ai.enemyObject.transform.position;
            ebt.ai.seekPointReached = false;
        }
        else
        {
            ebt.ai.seekLevelDsc = true;
        }

        DebugExtension.DebugWireSphere(ebt.ai.lastEnemyPosition, Color.cyan, 0.5f);

        return ebt.ai.enemyObject ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}