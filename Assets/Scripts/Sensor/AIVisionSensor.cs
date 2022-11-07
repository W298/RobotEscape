using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVisionSensor : MonoBehaviour
{
    public float redZoneDistance = 8.5f;
    public float yellowZoneDistance = 12.5f;

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

    [Header("Result")] 
    public List<GameObject> yellowZoneObjectList = new();
    public List<GameObject> redZoneObjectList = new();

    private void Ray(float currentVAngle, float currentHAngle, int v, int h, float distance, List<GameObject> objectList)
    {
        Vector3 point = transform.position +
                        Quaternion.AngleAxis(currentVAngle, transform.right) * Quaternion.AngleAxis(currentHAngle, transform.up) * transform.forward * distance;

        Physics.Linecast(transform.position, point, out RaycastHit hit, 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Object") | 1 << LayerMask.NameToLayer("Ground"));

        if ((h == 0 || h == horizontalResolution - 1) && (v == 0 || v == verticalResolution - 1))
        {
            Debug.DrawLine(transform.position, hit.collider ? hit.point : point, Color.white, scanInterval);
        }

        DebugExtension.DebugPoint(point, Color.white, 0.25f, scanInterval);

        if (hit.collider && hit.collider.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            var detectedObject = hit.collider.gameObject;
            if (!objectList.Contains(detectedObject)) objectList.Add(detectedObject);
        }
    }

    private void Scan()
    {
        yellowZoneObjectList.Clear();
        redZoneObjectList.Clear();

        float currentVAngle = -verticalAngle;
        float deltaVAngle = (verticalAngle * 2) / (verticalResolution - 1);
        for (int v = 0; v < verticalResolution; v++)
        {
            float currentHAngle = -horizontalAngle;
            float deltaHAngle = (horizontalAngle * 2) / (horizontalResolution - 1);
            for (int h = 0; h < horizontalResolution; h++)
            {
                Ray(currentVAngle, currentHAngle, v, h, yellowZoneDistance, yellowZoneObjectList);
                Ray(currentVAngle, currentHAngle, v, h, redZoneDistance, redZoneObjectList);

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
