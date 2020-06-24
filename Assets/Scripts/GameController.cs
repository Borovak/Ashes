using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
    public static event Action<ChamberController> ChamberChanged;
    public static GameController Instance;
    public static ChamberController currentChamber;
    public static FadeInOutController fadeInOutController;

    public Light2D backgroundLight;
    public Light2D terrainLight;
    public Vector3[] campsiteLocations;
    public GameObject campsitePrefab;

    private static Transform _chambersFolder;
    private static string _chamberResourcePathToLoad;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        if (!Application.isPlaying) return;
        SaveSystem.Load(true);
        InitCampsites();
    }

    void Start()
    {
        _chambersFolder = GameObject.FindGameObjectWithTag("ChambersFolder").transform;
        fadeInOutController = GameObject.FindGameObjectWithTag("FadeInOut").GetComponent<FadeInOutController>();
        if (Application.isPlaying)
        {
            for (int i = 0; i < _chambersFolder.transform.childCount; i++)
            {
                GameObject.Destroy(_chambersFolder.GetChild(i).gameObject);
            }
        }
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

    public static void ChangeChamber(string resourcePath)
    {
        //if (!Application.isPlaying) return;
        fadeInOutController = GameObject.FindGameObjectWithTag("FadeInOut").GetComponent<FadeInOutController>();
        _chamberResourcePathToLoad = resourcePath;
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

    private static void OnFadeOutCompleted()
    {
        _chambersFolder = GameObject.FindGameObjectWithTag("ChambersFolder").transform;
        for (int i = _chambersFolder.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(_chambersFolder.GetChild(i).gameObject);
        }
        var chamber = GameObject.Instantiate(Resources.Load<GameObject>(_chamberResourcePathToLoad), _chambersFolder);
        currentChamber = chamber.GetComponent<ChamberController>();
        if (!Application.isPlaying) return;
        if (currentChamber != null)
        {
            fadeInOutController.FadeOutCompleted -= OnFadeOutCompleted;
        }
        ChamberChanged?.Invoke(currentChamber);
    }
}
