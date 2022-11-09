using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    private RobotStatusController robotStatus;
    private EnemyRobotAI ai;
    private Camera mainCam;
    private Canvas canvas;
    private GameObject anchor;

    private RectTransform healthBorderRect;
    private RectTransform healthBarRect;

    private RectTransform detectBorderRect;
    private Image detectFrontImage;
    private RectTransform detectFrontRect;

    private bool SetPosition()
    {
        var screenPos = Camera.main.WorldToScreenPoint(anchor.transform.position + new Vector3(-0.5f, 0, 0.5f));
        var isInCamera = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos,
            canvas.worldCamera, out Vector2 movePos);

        detectBorderRect.position = canvas.transform.TransformPoint(movePos);
        healthBorderRect.position = canvas.transform.TransformPoint(movePos) + new Vector3(0, -30, 0);

        return isInCamera;
    }

    private void Start()
    {
        robotStatus = transform.root.GetComponent<RobotStatusController>();
        ai = transform.root.GetComponent<EnemyRobotAI>();
        mainCam = Camera.main;
        canvas = GetComponent<Canvas>();
        anchor = transform.root.Find("Anchor").gameObject;

        healthBorderRect = transform.GetChild(0).GetComponent<RectTransform>();
        healthBarRect = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();

        detectBorderRect = transform.GetChild(1).GetComponent<RectTransform>();
        detectFrontImage = detectBorderRect.transform.GetChild(1).GetComponent<Image>();
        detectFrontRect = detectBorderRect.transform.GetChild(1).GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        bool isInCamera = SetPosition();
        
        healthBorderRect.gameObject.SetActive(isInCamera);
        healthBarRect.sizeDelta = new Vector2(robotStatus.health, healthBarRect.sizeDelta.y);

        if (ai.detectLevel.currentLevel <= 0 || !isInCamera)
        {
            detectBorderRect.gameObject.SetActive(false);
        }
        else
        {
            detectBorderRect.gameObject.SetActive(true);
            detectFrontRect.sizeDelta = new Vector2(detectFrontRect.sizeDelta.x, Mathf.Clamp(ai.detectLevel.currentLevel * 0.3f * 1.25f, 0, 30));

            detectFrontImage.color = ai.detectLevel.currentLevel >= 80 ? new Color(0.735849f, 0.2249199f, 0.2662449f) : new Color(1, 0.7328318f, 0);
        }
    }
}
