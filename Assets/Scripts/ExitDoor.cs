using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private bool open = false;

    public void OpenDoor()
    {
        open = true;
    }

    private void Update()
    {
        if (!open || Vector3.Distance(transform.GetChild(1).localPosition, new Vector3(0, 4, 0)) <= 0.2f) return;
        transform.GetChild(1).localPosition = Vector3.Lerp(transform.GetChild(1).localPosition, new Vector3(0, 4, 0),
            Time.deltaTime);
    }
}
