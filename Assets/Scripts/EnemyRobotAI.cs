using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class EnemyRobotAI : MonoBehaviour
{
    [NonSerialized] public RobotInputHandler inputHandler;
    [NonSerialized] public NavMeshAgent navAgent;
    [NonSerialized] public GunController gunController;
    [NonSerialized] public AIVisionSensor visonSensor;
    [NonSerialized] public AISoundSensor soundSensor;
    [NonSerialized] public RobotStatusController statusController;

    private EnemyRobotBT bt;

    [Header("Reaction")]
    public float reactionDelay = 0.5f;

    [Header("Detection")]
    public GameObject enemyObject = null;
    public Vector3 lastEnemyPosition;

    [Header("Hit")]
    public bool isHit = false;
    public float hitRememberDur = 1;

    [Header("Hear")] 
    public bool isHear = false;
    public float hearRememberDur = 1;

    [Header("Seek Level")]
    public bool seekLevelDsc = true;
    public float seekLevel = 0;
    private float seekLevelDscSpeed = 1;
    private float seekLevelDscTimer;

    [Header("Seek")]
    public bool seekPointReached = false;

    [Header("Patrol")] public GameObject patrolPoints;

    [Header("Cover")]
    public GameObject closestCoverPoint = null;

    private void Start()
    {
        inputHandler = GetComponent<RobotInputHandler>();
        navAgent = GetComponent<NavMeshAgent>();
        statusController = GetComponent<RobotStatusController>();
        gunController = GetComponentInChildren<GunController>();

        bt = GetComponent<EnemyRobotBT>();

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

    public IEnumerator HitReaction(GameObject shooter)
    {
        yield return new WaitForSeconds(reactionDelay);
        isHit = true;
        enemyObject = shooter;

        yield return new WaitForSeconds(hitRememberDur);
        isHit = false;
    }

    public IEnumerator SoundReaction(Vector3 soundPosition)
    {
        yield return new WaitForSeconds(reactionDelay);
        isHear = true;
        seekLevel = 100;
        lastEnemyPosition = soundPosition;
        seekPointReached = false;

        yield return new WaitForSeconds(hearRememberDur);
        isHear = false;
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

    public void OnDeath()
    {
        navAgent.enabled = false;
        bt.enabled = false;
    }
}
