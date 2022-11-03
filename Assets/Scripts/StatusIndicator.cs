using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    private RobotStatusController robotStatus;
    private EnemyRobotAI ai;
    private Camera mainCam;

    private RectTransform healthBarRect;

    private GameObject detectBorder;
    private Image detectFrontImage;
    private RectTransform detectFrontRect;

    private void Start()
    {
        robotStatus = transform.root.GetComponent<RobotStatusController>();
        ai = transform.root.GetComponent<EnemyRobotAI>();
        mainCam = Camera.main;

        healthBarRect = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();

        detectBorder = transform.GetChild(1).gameObject;
        detectFrontImage = detectBorder.transform.GetChild(1).GetComponent<Image>();
        detectFrontRect = detectBorder.transform.GetChild(1).GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        healthBarRect.sizeDelta = new Vector2(robotStatus.health, healthBarRect.sizeDelta.y);

        if (ai.detectLevel.currentLevel <= 0)
        {
            detectBorder.SetActive(false);
        }
        else
        {
            detectBorder.SetActive(true);
            detectFrontRect.sizeDelta = new Vector2(detectFrontRect.sizeDelta.x, Mathf.Clamp(ai.detectLevel.currentLevel * 0.3f * 1.25f, 0, 30));

            detectFrontImage.color = ai.detectLevel.currentLevel >= 80 ? new Color(0.735849f, 0.2249199f, 0.2662449f) : new Color(1, 0.7328318f, 0);
        }

        var toRotation = Quaternion.LookRotation(mainCam.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Euler(-toRotation.eulerAngles.x, 0, 0);

        transform.parent.position = transform.root.position + new Vector3(0, 2, 0.5f);
    }
}
