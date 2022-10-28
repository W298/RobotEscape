using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVisionSensor : MonoBehaviour
{
    public float distance = 10;

    [Header("Angle")]
    [Range(0, 180)]
    public float horizontalAngle = 20;
    [Range(0, 180)]
    public float verticalAngle = 20;

    [Header("Resolution")]
    [Range(0, 48)]
    public int horizontalResolution = 10;
    [Range(0, 48)]
    public int verticalResolution = 3;

    [Header("Interval")] 
    public float scanInterval = 0.5f;
    private float scanTimer;

    [Header("Result")] public List<GameObject> detectedObjectList = new();

    private void Scan()
    {
        detectedObjectList.Clear();

        float currentVAngle = -verticalAngle;
        float deltaVAngle = (verticalAngle * 2) / (verticalResolution - 1);
        for (int v = 0; v < verticalResolution; v++)
        {
            float currentHAngle = -horizontalAngle;
            float deltaHAngle = (horizontalAngle * 2) / (horizontalResolution - 1);
            for (int h = 0; h < horizontalResolution; h++)
            {
                Vector3 point = transform.position + Quaternion.Euler(currentVAngle, currentHAngle, 0) * transform.forward * distance;
                Physics.Linecast(transform.position, point, out RaycastHit hit, 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Object"));
                
                if ((h == 0 || h == horizontalResolution - 1) && v == 0) Debug.DrawLine(transform.position, hit.collider ? hit.point : point, Color.red, scanInterval);

                if (hit.collider && hit.collider.gameObject.layer == LayerMask.NameToLayer("Object"))
                {
                    var detectedObject = hit.collider.gameObject;
                    if (!detectedObjectList.Contains(detectedObject)) detectedObjectList.Add(detectedObject);
                }

                currentHAngle += deltaHAngle;
            }
            currentVAngle += deltaVAngle;
        }
    }

    private void FixedUpdate()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }
}
