using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRobotAI : MonoBehaviour
{
    [NonSerialized] public RobotInputHandler inputHandler;
    [NonSerialized] public NavMeshAgent navAgent;
    [NonSerialized] public GunController gunController;
    [NonSerialized] public AIVisionSensor visonSensor;
    [NonSerialized] public AISoundSensor soundSensor;

    [Header("Detect Enemy")]
    public GameObject enemyObject = null;
    public Vector3 lastEnemyPosition;

    [Header("Status")]
    public float health = 100;

    [Header("Seek Level")]
    public bool seekLevelDsc = true;
    public float seekLevel = 0;
    private float seekLevelDscSpeed = 1;
    private float seekLevelDscTimer;

    [Header("Seek")]
    public bool seekPointReached = false;

    [Header("Patrol")] public GameObject patrolPoints;

    private void Start()
    {
        inputHandler = GetComponent<RobotInputHandler>();
        navAgent = GetComponent<NavMeshAgent>();
        gunController = GetComponentInChildren<GunController>();

        visonSensor = GetComponentInChildren<AIVisionSensor>();
        soundSensor = GetComponentInChildren<AISoundSensor>();

        navAgent.updateRotation = false;

        seekLevelDscTimer = seekLevelDscSpeed;
    }

    private void FixedUpdate()
    {
        if (seekLevelDsc) DecreaseSeekLevel();
    }

    private void DecreaseSeekLevel()
    {
        seekLevelDscTimer -= Time.deltaTime;
        if (seekLevelDscTimer < 0)
        {
            seekLevelDscTimer += seekLevelDscSpeed;
            if (seekLevel > 0) seekLevel--;
        }
    }

    public bool StartMove(Vector3 target)
    {
        navAgent.isStopped = false;
        return navAgent.SetDestination(target);
    }

    public void StopMove()
    {
        navAgent.isStopped = true;
    }
}
