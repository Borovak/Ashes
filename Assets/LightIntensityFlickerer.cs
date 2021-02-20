using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightIntensityFlickerer : MonoBehaviour
{
    public float min;
    public float max;
    public float period;
    
    private Light2D _light;
    private bool _isDisabled;
    
    void OnEnable()
    {
        _light ??= GetComponent<Light2D>();
        _isDisabled = false;
        Change();
    }
    
    void OnDisable()
    {
        _isDisabled = true;
    }

    private void Change()
    {
        if (_isDisabled) return;
        var nextValue = UnityEngine.Random.Range(min, max);
        var lt = LeanTween.Framework.LeanTween.value(_light.intensity, nextValue, period).setOnUpdate(v => _light.intensity = v);
        lt.setOnComplete(Change);
    }
    
}
