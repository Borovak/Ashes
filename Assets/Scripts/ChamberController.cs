﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class ChamberController : MonoBehaviour
{
    public static event Action<string, string> ZoneChanged;
    public string chamberGuid;
    public LocationInformation.Chamber chamber => LocationInformation.Chambers[chamberGuid];
    public string chamberName;
    public LocationInformation.Zone zone => LocationInformation.Zones[chamber.ZoneGuid];
    public string zoneGuid => chamber.ZoneGuid;
    public float BackgroundLightIntensity;
    public Color BackgroundLightColor;
    public float TerrainLightIntensity;
    public Color TerrainLightColor;
    public GameObject chamberContainer;
    public Vector2 position;
    public Vector2 size;
    public float scale;
    [SerializeField] public int[,] map;
    [SerializeField] public string theme;
    [SerializeField] public float colorShiftR;
    [SerializeField] public float colorShiftG;
    [SerializeField] public float colorShiftB;
    public AudioClip ambientSound;



    private static LocationInformation.Zone _lastZoneEntered;
    private AudioSource _audioSource;
    private Transform _enemyFolder;
    private VirtualCameraPlayerBinding _virtualCameraPlayerBinding;
    private bool _isPlayerInsideChamber;
    private List<GameObject> _containersToEnableDisable;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _virtualCameraPlayerBinding = GetComponentInChildren<VirtualCameraPlayerBinding>();
        _enemyFolder = new GameObject("Enemies").transform;
        _enemyFolder.parent = transform;
        var tilemapRenderer = GetComponentInChildren<TilemapRenderer>();
        tilemapRenderer.enabled = false;
        //Find containers to enable/disable
        var tags = new[] { "DecorContainer", "EnvironmentContainer" };
        _containersToEnableDisable = new List<GameObject>();
        foreach (var tag in tags)
        {
            var obj = GameObject.FindGameObjectsWithTag("DecorContainer").FirstOrDefault(x => x.transform.parent.name == gameObject.name);
            if (obj == null) continue;
            _containersToEnableDisable.Add(obj);
        }
        foreach (var container in _containersToEnableDisable)
        {
            container?.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Spawning/despawning enemies
        if (_isPlayerInsideChamber && !_virtualCameraPlayerBinding.isPlayerInsideChamber)
        {
            Debug.Log($"Chamber {chamber.Name} exited");
            var enemiesToDelete = GlobalFunctions.FindChildrenWithTag(_enemyFolder.gameObject, "Enemy", false);
            foreach (var enemy in enemiesToDelete)
            {
                GameObject.Destroy(enemy);
            }
            foreach (var container in _containersToEnableDisable)
            {
                container?.SetActive(false);
            }
            _audioSource.Stop();
        }
        else if (!_isPlayerInsideChamber && _virtualCameraPlayerBinding.isPlayerInsideChamber)
        {
            Debug.Log($"Chamber {chamber.Name} entered, {chamber.Enemies.Count} enemies present");
            if (_lastZoneEntered == null || _lastZoneEntered.Guid != chamber.ZoneGuid)
            {
                _lastZoneEntered = chamber.Zone;
                ZoneChanged?.Invoke(chamber.ZoneName, chamber.Name);
            }
            foreach (var enemy in chamber.Enemies)
            {
                enemy.Instantiate(_enemyFolder, position, size, scale);
            }
            foreach (var container in _containersToEnableDisable)
            {
                container?.SetActive(true);
            }
            Apply();
        }
        else return;
        _isPlayerInsideChamber = _virtualCameraPlayerBinding.isPlayerInsideChamber;
    }

    void Apply()
    {
        if (ambientSound != null)
        {
            _audioSource.loop = true;
            _audioSource.clip = ambientSound;
            _audioSource.Play();
        }
        GlobalLightController.UpdateLights(BackgroundLightIntensity, BackgroundLightColor, TerrainLightIntensity, TerrainLightColor);
    }

    public void SetBasicSettings(string guid, Vector2 position, Vector2 size, float scale)
    {
        chamberGuid = guid;
        this.position = position;
        this.size = size;
        this.scale = scale;
        SetCollider();
    }

    private void SetCollider()
    {
        var minX = position.x;
        var maxX = position.x + size.x;
        var biasY = 50f - size.y;
        var minY = position.y + biasY;
        var maxY = position.y + size.y + biasY;
        var points = new List<Vector2>
        {
            { new Vector2(minX, minY) },
            { new Vector2(minX + 1f, minY) },
            { new Vector2(maxX, minY) },
            { new Vector2(maxX, maxY) },
            { new Vector2(minX, maxY) }
        };
        var colliderGameObject = new List<GameObject>(GameObject.FindGameObjectsWithTag("ChamberCollider")).Find(g => g.transform.IsChildOf(transform));
        var collider = colliderGameObject.GetComponent<PolygonCollider2D>();
        collider.SetPath(0, points);
    }
}
