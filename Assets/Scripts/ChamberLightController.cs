using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberLightController : MonoBehaviour
{
    public ChamberController chamberController;

    private UnityEngine.Experimental.Rendering.Universal.Light2D _light;
    private const float intensity = 2f;
    private const float intensityVariation = 0.5f;
    private const float intensityVariationSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chamberController == null) return;
        _light.color = chamberController.BackgroundLightColor;
        _light.intensity = intensity + Mathf.Sin(Time.time * intensityVariationSpeed) * intensityVariation;
    }
}
