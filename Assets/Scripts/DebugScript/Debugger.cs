using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using EnemyRobotAIState;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyAI;

    private void Start()
    {
        SROptions.Current.player = player;
        SROptions.Current.enemyAI = enemyAI;
    }
}

public partial class SROptions
{
    public GameObject player;
    public GameObject enemyAI;
}
