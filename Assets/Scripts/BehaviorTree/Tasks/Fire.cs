using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;
using UnityEngine.AI;

public class Fire : Node
{
    private EnemyRobotBT ebt;

    public Fire(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        ebt.ai.inputHandler.isFire = true;

        return NodeState.RUNNING;
    }
}
