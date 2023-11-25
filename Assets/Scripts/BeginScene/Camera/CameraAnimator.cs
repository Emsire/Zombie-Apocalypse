using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction overAction;
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void TurnLeft(UnityAction action)
    {
        overAction = action;
        animator.SetTrigger("Left");
    }

    public void TurnRight(UnityAction action)
    {
        overAction = action;
        animator.SetTrigger("Right");
    }

    public void PlayerOver()
    {
        overAction?.Invoke();
        overAction = null;
    }
}
