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

    public BoxMode boxMode = BoxMode.AMMO;
    public int amount = 3;

    private void Start()
    {
        animator = transform.root.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool success = false;
        switch (boxMode)
        {
            case BoxMode.AMMO:
                GunController g = other.GetComponentInChildren<GunController>();
                if (g && amount > 0)
                {
                    g.ammoSystem.remainAmmo += 30;
                    amount--;
                    success = true;
                }
                break;
            case BoxMode.HEALTH:
                PlayerInventory i = other.GetComponent<PlayerInventory>();
                if (i && amount > 0)
                {
                    i.AddItem("AidKit", 1);
                    amount--;
                    success = true;
                }
                break;
        }

        if (success) animator.SetTrigger("Interact");
    }
}
