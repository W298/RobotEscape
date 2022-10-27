using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadController : StateMachineBehaviour
{
    private bool aimReload = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aimReload = animator.GetBool("isAim");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!aimReload && animator.GetBool("isAim"))
        {
            animator.SetBool("isReload", false);
        }

        if (stateInfo.normalizedTime > 1)
        {
            animator.transform.GetComponentInChildren<GunController>().OnReload();
            animator.SetBool("isReload", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isReload", false);
    }
}