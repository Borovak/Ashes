using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerGlowingHandsController : MonoBehaviour
{
    public Light2D[] lights;
    public float intensityWhenActive = 1f;
    public float changeRate = 1f;
    public bool state;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetIntensity = state ? intensityWhenActive : 0f;
        var delta = changeRate * Time.deltaTime * (state ? 1f : -1f);
        foreach (var light in lights)
        {
            light.intensity = GlobalFunctions.Bound(light.intensity + delta, 0f, intensityWhenActive);
        }
    }
}