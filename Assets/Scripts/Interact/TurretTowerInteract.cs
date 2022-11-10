using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TurretTowerInteract : MonoBehaviour
{
    private PlayerUI playerUI;
    private Canvas canvas;
    private Text hackText;
    private Text percentText;
    private RectTransform rootRect;
    private RectTransform gageRect;

    private RobotInputHandler callerInput;
    private float gage = 0;
    private bool success = false;

    public List<EnemyTurretAI> turretList;

    public void Interact(GameObject caller)
    {
        playerUI.HideInteractUI();
        playerUI.SetInteractDescription("");
        canvas.gameObject.SetActive(true);
        callerInput = caller.GetComponent<RobotInputHandler>();
    }

    private void LocateUI()
    {
        var screenPos = Camera.main.WorldToScreenPoint(transform.parent.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos,
            canvas.worldCamera, out Vector2 movePos);

        rootRect.position = canvas.transform.TransformPoint(movePos + new Vector2(50, 50));
    }

    private void OnSuccess()
    {
        success = true;
        hackText.text = "SUCCESS";
        hackText.color = new Color(0, 255, 0);
        
        turretList.ForEach(turret =>
        {
            turret.Deactivate();
            playerUI.GetComponentInChildren<MissionController>()
                .SetMissionStatus("Main01_" + turret.name.Substring(7, 2), MissionStatus.COMPLETE);
        });
    }

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUI>();
        canvas = transform.parent.GetComponentInChildren<Canvas>(true);
        hackText = canvas.transform.GetChild(0).Find("HACK").GetComponent<Text>();
        percentText = canvas.transform.GetChild(0).Find("Percent").GetComponent<Text>();
        rootRect = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        gageRect = canvas.transform.GetChild(0).Find("Back").GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        gage = Mathf.Clamp(gage + Time.deltaTime * (callerInput != null && callerInput.holdInteract ? 8 : success ? 0 : -10), 0, 100);
        percentText.text = (int)gage + " %";
        gageRect.sizeDelta = new Vector2(gage * 1.65f, gageRect.sizeDelta.y);
        LocateUI();

        if (gage >= 100)
        {
            if (!success) OnSuccess();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerUI == null || other.name != "Player") return;
        playerUI.ShowInteractUI(transform.parent.gameObject);
        playerUI.SetInteractDescription("HOLD");
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerUI == null || other.name != "Player") return;
        canvas.gameObject.SetActive(false);
        playerUI.HideInteractUI();
        playerUI.SetInteractDescription("");
    }
}
