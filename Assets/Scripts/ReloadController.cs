using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadController : StateMachineBehaviour
{
    private GunController gunController;
    private bool aimReload = false;
    private bool soundExcuted = false;

    private void Reset(Animator animator)
    {
        animator.SetBool("isReload", false);
        soundExcuted = false;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gunController = animator.transform.GetComponentInChildren<GunController>();
        aimReload = animator.GetBool("isAim");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!aimReload && animator.GetBool("isAim"))
        {
            Reset(animator);
        }

        if (0.5 <= stateInfo.normalizedTime && stateInfo.normalizedTime <= 0.6 && !soundExcuted)
        {
            gunController.PlayReloadSound();
            soundExcuted = true;
        }

        if (stateInfo.normalizedTime > 1)
        {
            gunController.OnReload();
            Reset(animator);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Reset(animator);
    }
}
