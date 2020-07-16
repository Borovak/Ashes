using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{

    public event Action FadeOutCompleted;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }

    public void TriggerFadeOutCompleted(){
        FadeOutCompleted?.Invoke();
    }
    
}
