using System.Collections;
using System.Collections.Generic;
using LeanTween.Framework;
using UnityEngine;

public class SaveIconController : MonoBehaviour
{
    public float InDuration;
    public float Duration;
    public float OutDuration;

    private Animator _animator;


    void OnEnable()
    {
        _animator ??= GetComponent<Animator>();
        gameObject.transform.LeanScale(Vector3.zero, 0f);
        SaveSystem.GameSaved += OnGameSaved;
    }

    void OnDisable()
    {
        SaveSystem.GameSaved -= OnGameSaved;
    }

    private void OnGameSaved(bool healOnSave)
    {
        _animator ??= GetComponent<Animator>();
        _animator.SetTrigger("active");
        var lt = LeanTween.Framework.LeanTween.scale(gameObject, new Vector3(1f,1f,1f), InDuration);
        lt.setEase(LeanTweenType.easeInOutSine);
        lt.setOnComplete(() => {
            lt = LeanTween.Framework.LeanTween.scale(gameObject, new Vector3(1f,1f,1f), Duration);
            lt.setOnComplete(() => {
                lt = LeanTween.Framework.LeanTween.scale(gameObject, Vector3.zero, OutDuration);
                lt.setEase(LeanTweenType.easeInOutSine);
            });
        });
    }
}
