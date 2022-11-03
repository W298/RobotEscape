using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISoundSensor : MonoBehaviour
{
    private EnemyRobotAI ai;
    public float hearRange = 20f;

    [Header("Result")]
    public Vector3 lastDetectedPosition;
    public GameObject lastDetectedOwner;

    private void Start()
    {
        ai = transform.root.GetComponent<EnemyRobotAI>();
    }

    public void OnSoundHear(float soundRange, Vector3 soundPosition, GameObject owner)
    {
        if (Vector3.Distance(transform.position, soundPosition) > soundRange + hearRange) return;

        lastDetectedPosition = soundPosition;
        lastDetectedOwner = owner;
        StartCoroutine(ai.SoundReaction(soundPosition, owner));
    }
}
