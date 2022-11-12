using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISoundSensor : MonoBehaviour
{
    private EnemyRobotAI ai;

    [Header("Param")]
    public float hearRange = 20f;
    public float accuracy = 4f;

    [Header("Result")]
    public Vector3 lastDetectedPosition;
    public GameObject lastDetectedOwner;

    private void Start()
    {
        ai = transform.root.GetComponent<EnemyRobotAI>();
    }

    private Vector3 CreateRandomPoint(Vector3 targetPosition, float maxDistance)
    {
        Vector3 randomPoint = Random.insideUnitSphere * 10 + targetPosition;
        NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, maxDistance, NavMesh.AllAreas);
        return hit.position;
    }

    public void OnSoundHear(float soundRange, Vector3 soundPosition, GameObject owner)
    {
        if (Vector3.Distance(transform.position, soundPosition) > soundRange + hearRange) return;

        lastDetectedPosition = soundPosition;
        lastDetectedOwner = owner;
        StartCoroutine(ai.SoundReaction(CreateRandomPoint(soundPosition, accuracy), owner));
    }
}
