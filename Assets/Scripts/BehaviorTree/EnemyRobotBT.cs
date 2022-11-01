using System;
using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotBT : BehaviorTree
{
    [NonSerialized]
    public EnemyRobotAI ai;

    private void Awake()
    {
        ai = GetComponent<EnemyRobotAI>();
    }

    protected override Node CreateTree()
    {
        Node attackSequence = new Sequence(new List<Node>
        {
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
        });

        Node root = new Sequence(new List<Node>
        {
            new Clear(this),
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new IsHealthLow(this, 30),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new IsDetectEnemy(this),
                            new Selector(new List<Node>
                            {
                                new Sequence(new List<Node>
                                {
                                    new IsHealthLow(this, 20),
                                    attackSequence
                                }),
                                new Cover(this)
                            })
                        }),
                        new Cover(this)
                    })
                }),
                new Sequence(new List<Node>
                {
                    new IsDetectEnemy(this),
                    attackSequence
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
                    new Sequence(new List<Node>
                    {
                        new Walk(this),
                        new Patrol(this)
                    })
                })
            })
        });

        return root;
    }
}
