using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using UnityEngine;

public class IsDetectEnemy : Node
{
    private EnemyRobotBT ebt;

    public IsDetectEnemy(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        GameObject enemy = ebt.ai.visonSensor.detectedObjectList.Find(o => o.name == "Player");
        
        ebt.ai.enemyObject = enemy;
        if (enemy)
        {
            ebt.ai.seekLevel = 100;
            ebt.ai.seekLevelDsc = false;
            ebt.ai.lastEnemyPosition = enemy.transform.position;
            ebt.ai.seekPointReached = false;
        }
        else
        {
            ebt.ai.seekLevelDsc = true;
        }

        DebugExtension.DebugWireSphere(ebt.ai.lastEnemyPosition, Color.cyan, 0.5f);

        return enemy ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
