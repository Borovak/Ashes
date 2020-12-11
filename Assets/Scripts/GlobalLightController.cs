using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightController : MonoBehaviour
{
    private static event Action<float, Color, float, Color> LightsUpdated;

    public Light2D BackgroundLight;
    public Light2D TerrainLight;

    // Start is called before the first frame update
    void Start()
    {
        LightsUpdated += OnLightsUpdated;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnLightsUpdated(float backgroundIntensity, Color backgroundColor, float terrainIntensity, Color terrainColor)
    {
        BackgroundLight.intensity = backgroundIntensity;
        BackgroundLight.color = backgroundColor;
        TerrainLight.intensity = terrainIntensity;
        TerrainLight.color = terrainColor;
    }

    public static void UpdateLights(float backgroundIntensity, Color backgroundColor, float terrainIntensity, Color terrainColor)
    {
        LightsUpdated?.Invoke(backgroundIntensity, backgroundColor, terrainIntensity, terrainColor);
    }
}
