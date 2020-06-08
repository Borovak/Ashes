using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameController : MonoBehaviour
{
    public static event Action<ChamberController> ChamberChanged;
    public static GameController Instance;
    public ChamberController currentChamber;
    public Light2D backgroundLight;
    public Light2D terrainLight;
    public FadeInOutController fadeInOutController;
    public Vector3[] campsiteLocations;
    public GameObject campsitePrefab;

    private ChamberController _nextChamber;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        InitCampsites();
    }

    // Update is called once per frame
    void Update()
    {
        SetLighting();
    }

    private void InitCampsites()
    {
        var campsitesTransform = new GameObject("Campsites").transform;
        foreach (var location in campsiteLocations)
        {
            var campsite = GameObject.Instantiate(campsitePrefab, location, Quaternion.identity);
            campsite.transform.parent = campsitesTransform;
        }
    }

    private void SetLighting()
    {
        if (currentChamber == null) return;
        backgroundLight.intensity = currentChamber.BackgroundLightIntensity;
        backgroundLight.color = currentChamber.BackgroundLightColor;
        terrainLight.intensity = currentChamber.TerrainLightIntensity;
        terrainLight.color = currentChamber.TerrainLightColor;
    }

    public void ChangeChamber(ChamberController nextChamber)
    {
        if (currentChamber == nextChamber) return;
        Debug.Log($"New chamber entered: {nextChamber.transform.name}");
        _nextChamber = nextChamber;
        if (currentChamber != null)
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
        if (currentChamber != null)
        {
            fadeInOutController.FadeOutCompleted -= OnFadeOutCompleted;
            currentChamber?.transform.Find("Content").gameObject.SetActive(false);
        }
        _nextChamber.transform.Find("Content").gameObject.SetActive(true);
        currentChamber = _nextChamber;
        ChamberChanged?.Invoke(currentChamber);
        fadeInOutController.FadeIn();
    }
}
