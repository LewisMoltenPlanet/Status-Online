using UnityEngine;

public class ReloadState : StateMachineBehaviour
{

    public float reloadTime = 0.7f;

    bool hasReloaded = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasReloaded = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasReloaded)
            return;

        if (stateInfo.normalizedTime >= reloadTime)
        {
            animator.GetComponent<WeaponController>().Reload();
            hasReloaded = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasReloaded = false;
    }
}

