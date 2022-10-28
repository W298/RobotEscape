using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class IsSeekLevelHigh : Node
{
    private EnemyRobotBT ebt;
    private float threshold = 60;

    public IsSeekLevelHigh(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        return ebt.ai.seekLevel > threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
