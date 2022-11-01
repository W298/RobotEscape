using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDetectObject : Node
{
    private EnemyRobotBT ebt;

    public IsDetectObject(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        return ebt.ai.visonSensor.redZoneObjectList.Count != 0 ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
