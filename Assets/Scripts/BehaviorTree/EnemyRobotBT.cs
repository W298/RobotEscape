using System;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotBT : BTree
{
    [NonSerialized]
    public EnemyRobotAI ai;

    private void Awake()
    {
        ai = GetComponent<EnemyRobotAI>();
    }

    protected override Node CreateTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new Clear(this),
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new IsDetectEnemy(this),
                    new TakeDistance(this),
                    new Aim(this),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new NeedReload(this),
                            new Reload(this)
                        }),
                        new Fire(this)
                    })
                }),
                new Sequence(new List<Node>
                {
                    new NeedReload(this),
                    new Reload(this)
                }),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new IsSeekLevelHigh(this),
                        new Aim(this),
                        new Seek(this)
                    }),
                    new Patrol(this)
                })
            })
        });

        return root;
    }
}
