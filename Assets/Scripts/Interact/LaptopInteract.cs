using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaptopInteract : MonoBehaviour
{
    private PlayerUI playerUI;
    private Canvas canvas;
    private Text hackText;
    private Text percentText;
    private RectTransform rootRect;
    private RectTransform gageRect;
    private AudioSource audioSource;

    private RobotInputHandler callerInput;
    private float gage = 0;
    private bool success = false;

    public Material onMaterial;

    public void Interact(GameObject caller)
    {
        playerUI.HideInteractUI();
        playerUI.SetInteractDescription("");
        canvas.gameObject.SetActive(true);
        callerInput = caller.GetComponent<RobotInputHandler>();

        transform.parent.GetComponent<MeshRenderer>().material = onMaterial;
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
        hackText.color = new Color(0, 1, 0);

        playerUI.GetComponentInChildren<MissionController>().SetMissionStatus("Main02", MissionStatus.COMPLETE);
    }

    private void StartBeep()
    {
        audioSource.Play();

        foreach (var robot in GameObject.FindGameObjectsWithTag("Robot"))
        {
            var soundSensor = robot.GetComponentInChildren<AISoundSensor>();
            if (soundSensor) soundSensor.OnSoundHear(audioSource.maxDistance, transform.parent.position, transform.parent.gameObject, true);
        }

        StartCoroutine(StopBeep());
    }

    private IEnumerator StopBeep()
    {
        yield return new WaitForSeconds(0.8f * 10);
        audioSource.Stop();
    }

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUI>();
        canvas = transform.parent.GetComponentInChildren<Canvas>(true);
        hackText = canvas.transform.GetChild(0).Find("HACK").GetComponent<Text>();
        percentText = canvas.transform.GetChild(0).Find("Percent").GetComponent<Text>();
        rootRect = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        gageRect = canvas.transform.GetChild(0).Find("Back").GetChild(0).GetComponent<RectTransform>();
        audioSource = transform.parent.GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        gage = Mathf.Clamp(gage + Time.deltaTime * (callerInput != null && callerInput.holdInteract ? 4 : 0), 0, 100);
        percentText.text = (int)gage + " %";
        gageRect.sizeDelta = new Vector2(gage * 1.65f, gageRect.sizeDelta.y);
        LocateUI();

        if (((30 <= gage && gage <= 31) || (80 <= gage && gage <= 81)) && !audioSource.isPlaying)
        {
            StartBeep();
        }

        if (gage >= 100)
        {
            if (!success) OnSuccess();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerUI == null || other.name != "Player") return;
        playerUI.ShowInteractUI(transform.parent.gameObject);
        playerUI.SetInteractDescription("HACK");
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerUI == null || other.name != "Player") return;
        canvas.gameObject.SetActive(false);
        playerUI.HideInteractUI();
        playerUI.SetInteractDescription("");
    }
}
