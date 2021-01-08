using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{

    public static event Action FadeOutCompleted;

    private static Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void FadeIn()
    {
        _animator.SetTrigger("FadeIn");
    }

    public static void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }

    public static void TriggerFadeOutCompleted(){
        FadeOutCompleted?.Invoke();
    }
    
}
