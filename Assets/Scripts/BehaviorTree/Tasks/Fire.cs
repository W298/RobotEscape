using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class Fire : Node
{
    private EnemyRobotBT ebt;

    public Fire(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        ebt.ai.inputHandler.isFire = true;

        return NodeState.RUNNING;
    }
}
