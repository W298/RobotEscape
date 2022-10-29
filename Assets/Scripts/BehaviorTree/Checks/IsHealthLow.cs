using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class IsHealthLow : Node
{
    private EnemyRobotBT ebt;
    private float threshold = 30;

    public IsHealthLow(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        return ebt.ai.statusController.health < threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
