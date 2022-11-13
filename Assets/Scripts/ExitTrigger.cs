using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public bool active = false;

    private void OnTriggerEnter()
    {
        if (!active) return;
        Time.timeScale = 0;
        FindObjectOfType<PlayerUI>().OnSuccess();
    }
}
