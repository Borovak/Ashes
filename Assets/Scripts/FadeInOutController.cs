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

    public void FadeIn(EntrancePoint.EntranceTypes entranceType)
    {
        switch (entranceType)
        {
            case EntrancePoint.EntranceTypes.Left:
                _animator.SetInteger("FadeInType", 2);
                break;
            case EntrancePoint.EntranceTypes.Right:
                _animator.SetInteger("FadeInType", 3);
                break;
            default:
                _animator.SetInteger("FadeInType", 1);
                break;
        }
    }

    public void FadeOut(EntrancePoint.EntranceTypes entranceType)
    {
        switch (entranceType)
        {
            case EntrancePoint.EntranceTypes.Left:
                _animator.SetInteger("FadeOutType", 2);
                break;
            case EntrancePoint.EntranceTypes.Right:
                _animator.SetInteger("FadeOutType", 3);
                break;
            default:
                _animator.SetInteger("FadeOutType", 1);
                break;
        }
    }

    public void TriggerFadeOutCompleted(){
        FadeOutCompleted?.Invoke();
    }
    
}
