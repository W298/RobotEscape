using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private GunController gunController;
    private RobotStatusController statusController;
    private PlayerInventory playerInventory;
    private Animator animator;

    private Canvas canvas;
    private Text ammoText;
    private GameObject ammoContainer;
    private Text healthText;
    private GameObject healthContainer;
    private Text aidText;
    private Text reloadText;
    private RectTransform interactRect;
    private Text descriptionText;

    public GameObject interactObject = null;

    public GameObject ammoIconPrefab;

    public void ShowInteractUI(GameObject interactObject)
    {
        this.interactObject = interactObject;
    }

    public void HideInteractUI()
    {
        this.interactObject = null;
    }

    public void SetInteractDescription(string text)
    {
        descriptionText.text = text;
    }

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        gunController = player.GetComponentInChildren<GunController>();
        statusController = player.GetComponentInChildren<RobotStatusController>();
        playerInventory = player.GetComponent<PlayerInventory>();
        animator = player.GetComponent<Animator>();

        canvas = transform.GetComponent<Canvas>();
        ammoText = transform.Find("AmmoText").GetComponent<Text>();
        ammoContainer = transform.Find("AmmoContainer").gameObject;
        healthText = transform.Find("HealthText").GetComponent<Text>();
        healthContainer = transform.Find("HealthContainer").gameObject;
        aidText = transform.Find("AIDText").GetComponent<Text>();
        reloadText = transform.Find("ReloadingText").GetComponent<Text>();
        interactRect = transform.Find("Interact").GetComponent<RectTransform>();
        descriptionText = transform.Find("Interact").Find("DescriptionImage").GetChild(0).GetComponent<Text>();
    }

    private void LateUpdate()
    {
        reloadText.gameObject.SetActive(animator.GetBool("isReload"));

        ammoText.text = gunController.ammoSystem.magAmmo + " / " + gunController.ammoSystem.remainAmmo;
        healthText.text = ((int)statusController.health).ToString();

        interactRect.gameObject.SetActive(interactObject != null);
        if (interactObject)
        {
            var screenPos = Camera.main.WorldToScreenPoint(interactObject.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos,
                canvas.worldCamera, out Vector2 movePos);

            interactRect.position = canvas.transform.TransformPoint(movePos + new Vector2(50, 50));
        }

        var aidKit = playerInventory.GetItem("AidKit");
        aidText.text = "x " + (aidKit != null ? aidKit.count.ToString() : "0");

        int count = gunController.ammoSystem.magAmmo - ammoContainer.transform.childCount;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject ammoIcon = Instantiate(ammoIconPrefab, ammoContainer.transform);
                ammoIcon.GetComponent<RectTransform>().localPosition -= new Vector3(10 * (ammoContainer.transform.childCount - 1), 0, 0);
            }
        }
        else if (count < 0)
        {
            for (int i = 0; i < -1 * count; i++)
            {
                Destroy(ammoContainer.transform.GetChild(ammoContainer.transform.childCount - 1).gameObject);
            }
        }

        int segmentCount = (int)(statusController.health /
                           (statusController.maxHealth / healthContainer.transform.childCount));

        for (int i = 0; i < segmentCount; i++)
        {
            healthContainer.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = segmentCount; i < healthContainer.transform.childCount; i++)
        {
            healthContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
