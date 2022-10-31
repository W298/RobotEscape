using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class IsHealthLow : Node
{
    private EnemyRobotBT ebt;
    private float threshold;

    public IsHealthLow(BehaviorTree bt, float threshold) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        return (ebt.ai.statusController.health <= threshold) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
