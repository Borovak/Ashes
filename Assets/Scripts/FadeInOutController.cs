using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{
    public event Action FadeOutCompleted;
    public float transitionTime;
    public float timeRemaining;
    public float x;
    public float y;
    public AnimationCurve opacity;
    public bool isFadeIn;

    private UnityEngine.UI.Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            x = (transitionTime - timeRemaining) / transitionTime;
            y = Mathf.Clamp01(opacity.Evaluate(x));
            y = isFadeIn ? 1f - y : y;
            SetAlpha(y);
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0){
                if (isFadeIn){
                    PhysicsObject.PhysicsEnabled = true;
                } else {
                    FadeOutCompleted?.Invoke();
                    FadeIn();
                }
            }
        }
        else if (_image.color.a > 0)
        {
            SetAlpha(0f);
        }
    }

    private void SetAlpha(float a)
    {
        var c = Color.black;
        c.a = a;
        _image.color = c;
    }

    public void FadeOut()
    {
        Debug.Log("Fade out");
        PhysicsObject.PhysicsEnabled = false;
        timeRemaining = transitionTime;
        isFadeIn = false;
    }

    public void FadeIn()
    {
        Debug.Log("Fade in");
        timeRemaining = transitionTime;
        isFadeIn = true;
    }
}
