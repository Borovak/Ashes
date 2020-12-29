using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimatorTriggerSetter : MonoBehaviour
{
    public Animator animator;

    public void Apply(int value)
    {
        animator.SetInteger("Index", value);
        animator.SetBool("OK", true);
    }
}
