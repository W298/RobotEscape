using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class Seek : Node
{
    private EnemyRobotBT ebt;
    
    private float seekWaitDuration = 3;
    private float seekWaitTimer;
    private Vector3 seekPosition;

    public Seek(BTree bt) : base(bt)
    {
        ebt = (EnemyRobotBT)bt;
        seekWaitTimer = seekWaitDuration;
    }

    public override NodeState Evaluate()
    {
        if (!ebt.ai.seekPointReached) seekPosition = ebt.ai.lastEnemyPosition;

        float remainDistance = Vector3.Distance(ebt.ai.navAgent.transform.position, seekPosition);
        if (remainDistance <= ebt.ai.navAgent.stoppingDistance)
        {
            seekWaitTimer -= Time.deltaTime;
            if (seekWaitTimer < 0)
            {
                seekWaitTimer += seekWaitDuration;

                seekPosition = CreateRandomPoint();
                ebt.ai.seekPointReached = true;
            }
        }

        bool success = ebt.ai.StartMove(seekPosition);
        DebugExtension.DebugWireSphere(seekPosition, Color.green, 0.5f);

        return success ? NodeState.RUNNING : NodeState.FAILURE;
    }

    private Vector3 CreateRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 10 + ebt.ai.lastEnemyPosition;

        NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 10, NavMesh.AllAreas);

        return hit.position;
    }
}
