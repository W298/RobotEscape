using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class Reload : Node
{
    private EnemyRobotBT ebt;

    public Reload(BehaviorTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
    }

    public override NodeState Evaluate()
    {
        if (!ebt.ai.GetComponent<Animator>().GetBool("isReload"))
        {
            ebt.ai.inputHandler.Reload();
        }

        return NodeState.RUNNING;
    }
}
