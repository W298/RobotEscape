using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BT;
using UnityEngine;

public class IsDetectEnemy : Node
{
    private EnemyRobotBT ebt;

    private Timer detectLevelStayTimer;

    public IsDetectEnemy(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;

        detectLevelStayTimer = new Timer(2f, () =>
        {
            ebt.ai.detectLevel.decTimer.active = true;

            detectLevelStayTimer.active = false;
            detectLevelStayTimer.Reset();
        });
        detectLevelStayTimer.active = false;
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
                    ebt.ai.detectLevel.incTimer.active = true;
                    ebt.ai.detectLevel.decTimer.active = false;

                    if (ebt.ai.detectLevel.currentLevel >= 80)
                    {
                        ebt.ai.enemyObject = yellowZoneEnemy;
                    }
                }
                else
                {
                    if (ebt.ai.detectLevel.incTimer.active)
                    {
                        detectLevelStayTimer.active = true;
                    }

                    ebt.ai.detectLevel.incTimer.active = false;
                }
            }
            else
            {
                ebt.ai.detectLevel.currentLevel = 100;
            }
        }

        if (ebt.ai.enemyObject)
        {
            ebt.ai.seekLevel.currentLevel = 100;
            ebt.ai.seekLevel.decTimer.active = false;
            ebt.ai.lastEnemyPosition = ebt.ai.enemyObject.transform.position;
            ebt.ai.seekPointReached = false;
            ebt.ai.closestCoverPoint = null;
        }
        else
        {
            ebt.ai.seekLevel.decTimer.active = true;
        }

        detectLevelStayTimer.Update();
        
        DebugExtension.DebugWireSphere(ebt.ai.lastEnemyPosition, Color.cyan, 0.5f);

        return ebt.ai.enemyObject ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
