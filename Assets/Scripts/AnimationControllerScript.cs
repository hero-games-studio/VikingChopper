using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerScript : MonoBehaviour
{
    public Animator playerAnimator;

    public void triggerPulling(){
        playerAnimator.SetTrigger("pullTrigger");
    }

    public void triggerThrow(){
        playerAnimator.SetTrigger("throwTrigger");
    }

    public void triggerIdle(){
        playerAnimator.SetTrigger("idleTrigger");
    }
}
