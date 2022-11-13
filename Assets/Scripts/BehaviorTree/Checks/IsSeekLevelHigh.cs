using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class IsSeekLevelHigh : Node
{
    private EnemyRobotBT ebt;
    private float threshold = 60;

    public IsSeekLevelHigh(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        if (ebt.ai.seekLevel.currentLevel > threshold)
        {
            return NodeState.SUCCESS;
        }

        ebt.ai.lastEnemyPosition = new Vector3(-100, -100, -100);
        ebt.ai.soundSensor.lastDetectedPosition = new Vector3(-100, -100, -100);
        return NodeState.FAILURE;
    }
}
