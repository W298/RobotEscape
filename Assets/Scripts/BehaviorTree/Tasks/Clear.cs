using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class Clear : Node
{
    private EnemyRobotBT ebt;

    public Clear(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        ebt.ai.inputHandler.isAim = false;
        ebt.ai.inputHandler.isFire = false;
        ebt.ai.navAgent.speed = ebt.ai.inputHandler.maxSpeed;
        ebt.ai.StopMove();

        return NodeState.SUCCESS;
    }
}
