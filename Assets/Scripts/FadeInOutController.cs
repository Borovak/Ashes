using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{

    public static event Action FadeOutCompleted;

    private static Animator _animator;
    private static Action _onFadeOutCompletedAction;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public static void FadeIn()
    {
        //Debug.Log("Fade in");
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animator.SetTrigger("FadeIn");
        }
    }

    public static void FadeOut(Action onFadeOutCompletedAction)
    {
        //Debug.Log("Fade out");
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Black"))
        {
            _onFadeOutCompletedAction = onFadeOutCompletedAction;
            _animator.SetTrigger("FadeOut");
            return;
        }
        onFadeOutCompletedAction?.Invoke();
    }

    public static void TriggerFadeOutCompleted()
    {
        FadeOutCompleted?.Invoke();
        _onFadeOutCompletedAction?.Invoke();
        _onFadeOutCompletedAction = null;
    }

}
