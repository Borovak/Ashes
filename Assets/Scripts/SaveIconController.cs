using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveIconController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
        var lt = LeanTween.scale(gameObject, new Vector3(1f,1f,1f), 1.2f);
        lt.setEase(LeanTweenType.easeInOutSine);
        lt.setOnComplete(() => {
            lt = LeanTween.scale(gameObject, new Vector3(1f,1f,1f), 1.2f);
            lt.setOnComplete(() => {
                lt = LeanTween.scale(gameObject, Vector3.zero, 1.2f);
                lt.setEase(LeanTweenType.easeInOutSine);
            });
        });
    }
}
