using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISoundSensor : MonoBehaviour
{
    public float hearRange = 20f;

    [Header("Result")] 
    public Vector3 lastDetectedPosition;
    public GameObject lastDetectedOwner;

    public void FixedUpdate()
    {
        DebugExtension.DebugWireSphere(transform.position, Color.cyan, hearRange);
    }

    public void OnSoundHear(float soundRange, Vector3 soundPosition, GameObject owner)
    {
        if (Vector3.Distance(transform.position, soundPosition) > soundRange + hearRange) return;

        lastDetectedPosition = soundPosition;
        lastDetectedOwner = owner;
    }
}
