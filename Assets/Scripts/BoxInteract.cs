using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxMode
{
    AMMO,
    HEALTH
}

public class BoxInteract : MonoBehaviour
{
    private Animator animator;
    private PlayerUI playerUI;
    private bool isActive = false;

    public BoxMode boxMode = BoxMode.AMMO;
    public int amount = 3;

    public void Interact(GameObject target)
    {
        if (!isActive) return;

        bool success = false;
        switch (boxMode)
        {
            case BoxMode.AMMO:
                GunController g = target.GetComponentInChildren<GunController>();
                if (g && amount > 0)
                {
                    g.ammoSystem.remainAmmo += 30;
                    amount--;
                    success = true;
                }
                break;
            case BoxMode.HEALTH:
                PlayerInventory i = target.GetComponent<PlayerInventory>();
                if (i && amount > 0)
                {
                    i.AddItem("AidKit", 1);
                    amount--;
                    success = true;
                }
                break;
        }

        if (success) animator.SetTrigger("Interact");
        playerUI.SetInteractDescription(amount + " remain");
    }

    private void Start()
    {
        animator = transform.root.GetComponent<Animator>();
        playerUI = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player") return;
        isActive = true;
        playerUI.ShowInteractUI(transform.parent.gameObject);
        playerUI.SetInteractDescription(amount + " remain");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != "Player") return;
        isActive = false;
        playerUI.HideInteractUI();
        playerUI.SetInteractDescription("");
    }
}
