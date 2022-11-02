using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    private RobotStatusController robotStatus;
    private RectTransform healthBarRect;

    private void Start()
    {
        robotStatus = transform.root.GetComponent<RobotStatusController>();
        healthBarRect = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        healthBarRect.sizeDelta = new Vector2(robotStatus.health, healthBarRect.sizeDelta.y);


        var toRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Euler(-toRotation.eulerAngles.x, 0, 0);
    }
}
