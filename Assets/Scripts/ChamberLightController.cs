using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberLightController : MonoBehaviour
{
    public ChamberController chamberController;

    private UnityEngine.Experimental.Rendering.Universal.Light2D _light;
    private const float Intensity = 2f;
    private const float IntensityVariation = 0.5f;
    private const float IntensityVariationSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chamberController == null) return;
        _light.color = chamberController.backgroundLightColor;
        _light.intensity = Intensity + Mathf.Sin(Time.time * IntensityVariationSpeed) * IntensityVariation;
    }
}
