using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameController : MonoBehaviour
{
    public static event Action<ChamberController> ChamberChanged;
    public static GameController Instance;
    public ChamberController CurrentChamber;
    public Light2D BackgroundLight;
    public Light2D TerrainLight;
    public FadeInOutController fadeInOutController;

    private ChamberController _nextChamber;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        SetLighting();
    }

    private void SetLighting()
    {
        if (CurrentChamber == null) return;
        BackgroundLight.intensity = CurrentChamber.BackgroundLightIntensity;
        BackgroundLight.color = CurrentChamber.BackgroundLightColor;
        TerrainLight.intensity = CurrentChamber.TerrainLightIntensity;
        TerrainLight.color = CurrentChamber.TerrainLightColor;
    }

    public void ChangeChamber(ChamberController nextChamber)
    {
        if (CurrentChamber == nextChamber) return;
        Debug.Log($"New chamber entered: {nextChamber.transform.name}");
        _nextChamber = nextChamber;
        if (CurrentChamber != null)
        {
            fadeInOutController.FadeOutCompleted += OnFadeOutCompleted;
            fadeInOutController.FadeOut();
        }
        else
        {
            OnFadeOutCompleted();
        }
    }

    private void OnFadeOutCompleted()
    {
        if (CurrentChamber != null)
        {
            fadeInOutController.FadeOutCompleted -= OnFadeOutCompleted;
            CurrentChamber?.transform.Find("Content").gameObject.SetActive(false);
        }
        _nextChamber.transform.Find("Content").gameObject.SetActive(true);
        CurrentChamber = _nextChamber;
        ChamberChanged?.Invoke(CurrentChamber);
        fadeInOutController.FadeIn();
    }
}
