using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class NeedReload : Node
{
    private EnemyRobotBT ebt;

    public NeedReload(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        bool needReload = ebt.ai.gunController.ammoSystem.magAmmo <= 0;
        return needReload ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
