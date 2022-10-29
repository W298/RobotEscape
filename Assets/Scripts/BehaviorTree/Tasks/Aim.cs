using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class Aim : Node
{
    private EnemyRobotBT ebt;

    public Aim(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        ebt.ai.inputHandler.isAim = true;
        ebt.ai.navAgent.speed = ebt.ai.inputHandler.maxSpeed / 2;

        return NodeState.SUCCESS;
    }
}
