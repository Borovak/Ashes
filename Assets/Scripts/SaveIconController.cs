using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveIconController : MonoBehaviour
{
    public float InDuration;
    public float Duration;
    public float OutDuration;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        gameObject.transform.LeanScale(Vector3.zero, 0f);
    }

    void OnEnable()
    {
        SaveSystem.GameSaved += OnGameSaved;
    }

    void OnDisable()
    {
        SaveSystem.GameSaved -= OnGameSaved;
    }

    private void OnGameSaved(bool healOnSave)
    {
        _animator.SetTrigger("active");
        var lt = LeanTween.scale(gameObject, new Vector3(1f,1f,1f), InDuration);
        lt.setEase(LeanTweenType.easeInOutSine);
        lt.setOnComplete(() => {
            lt = LeanTween.scale(gameObject, new Vector3(1f,1f,1f), Duration);
            lt.setOnComplete(() => {
                lt = LeanTween.scale(gameObject, Vector3.zero, OutDuration);
                lt.setEase(LeanTweenType.easeInOutSine);
            });
        });
    }
}
