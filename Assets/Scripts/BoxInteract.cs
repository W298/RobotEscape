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
    public BoxMode boxMode = BoxMode.AMMO;

    private void OnTriggerEnter(Collider other)
    {
        switch (boxMode)
        {
            case BoxMode.AMMO:
                GunController g = other.GetComponentInChildren<GunController>();
                if (g) g.ammoSystem.remainAmmo += 60;
                break;
            case BoxMode.HEALTH:
                break;
        }
    }
}
