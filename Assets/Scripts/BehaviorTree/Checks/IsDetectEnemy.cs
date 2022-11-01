using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BT;
using UnityEngine;

public class IsDetectEnemy : Node
{
    private EnemyRobotBT ebt;

    private float seekLevelAscSpeed = 0.05f;
    private float seekLevelAscTimer;

    public IsDetectEnemy(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;

        seekLevelAscTimer = seekLevelAscSpeed;
    }

    public override NodeState Evaluate()
    {
        if (!ebt.ai.isHit)
        {
            var redZoneEnemy = ebt.ai.visonSensor.redZoneObjectList.Find(o => o != null && !o.GetComponent<RobotStatusController>().isDeath && o.name == "Player");

            ebt.ai.enemyObject = redZoneEnemy;

            if (!redZoneEnemy)
            {
                var yellowZoneEnemy = ebt.ai.visonSensor.yellowZoneObjectList.Find(o =>
                    o != null && !o.GetComponent<RobotStatusController>().isDeath && o.name == "Player");

                if (yellowZoneEnemy)
                {
                    ebt.ai.seekLevelDsc = false;

                    seekLevelAscTimer -= Time.deltaTime;
                    if (seekLevelAscTimer < 0)
                    {
                        seekLevelAscTimer += seekLevelAscSpeed;
                        ebt.ai.seekLevel++;
                        ebt.ai.lastEnemyPosition = yellowZoneEnemy.transform.position;
                    }

                    if (ebt.ai.seekLevel >= 80)
                    {
                        ebt.ai.enemyObject = yellowZoneEnemy;
                    }
                }
            }
        }

        if (ebt.ai.enemyObject)
        {
            ebt.ai.seekLevel = 100;
            ebt.ai.seekLevelDsc = false;
            ebt.ai.lastEnemyPosition = ebt.ai.enemyObject.transform.position;
            ebt.ai.seekPointReached = false;
            ebt.ai.closestCoverPoint = null;
        }
        else
        {
            ebt.ai.seekLevelDsc = true;
        }

        DebugExtension.DebugWireSphere(ebt.ai.lastEnemyPosition, Color.cyan, 0.5f);

        return ebt.ai.enemyObject ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
