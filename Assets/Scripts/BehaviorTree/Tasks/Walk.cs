using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : Node
{
    private EnemyRobotBT ebt;

    public Walk(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        ebt.ai.inputHandler.isWalk = true;
        ebt.ai.navAgent.speed = ebt.ai.inputHandler.maxSpeed / 3;

        return NodeState.SUCCESS;
    }
}
