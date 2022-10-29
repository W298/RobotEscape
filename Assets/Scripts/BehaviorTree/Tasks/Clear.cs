using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class Clear : Node
{
    private EnemyRobotBT ebt;

    public Clear(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        ebt.ai.inputHandler.isAim = false;
        ebt.ai.inputHandler.isFire = false;
        ebt.ai.inputHandler.isWalk = false;
        ebt.ai.navAgent.speed = ebt.ai.inputHandler.maxSpeed;
        ebt.ai.StopMove();

        return NodeState.SUCCESS;
    }
}
