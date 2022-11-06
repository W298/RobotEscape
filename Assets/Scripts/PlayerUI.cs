using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private GunController gunController;
    private RobotStatusController statusController;
    private PlayerInventory playerInventory;
    
    private Text ammoText;
    private GameObject ammoContainer;
    private Text healthText;
    private GameObject healthContainer;
    private Text aidText;

    public GameObject ammoIconPrefab;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        gunController = player.GetComponentInChildren<GunController>();
        statusController = player.GetComponentInChildren<RobotStatusController>();
        playerInventory = player.GetComponent<PlayerInventory>();

        ammoText = transform.GetChild(0).GetComponent<Text>();
        ammoContainer = transform.GetChild(1).gameObject;
        healthText = transform.GetChild(3).GetComponent<Text>();
        healthContainer = transform.GetChild(4).gameObject;
        aidText = transform.GetChild(6).GetComponent<Text>();
    }

    private void LateUpdate()
    {
        ammoText.text = gunController.ammoSystem.magAmmo + " / " + gunController.ammoSystem.remainAmmo;
        healthText.text = ((int)statusController.health).ToString();

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
