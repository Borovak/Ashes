using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ChamberController : MonoBehaviour
{
    public string chamberGuid;
    public string chamberName => LocationInformation.Chambers[chamberGuid].Name;
    public string zoneGuid => LocationInformation.Chambers[chamberGuid].ZoneGuid;
    public string zoneName => LocationInformation.Chambers[chamberGuid].ZoneName;
    public float BackgroundLightIntensity;
    public Color BackgroundLightColor;
    public float TerrainLightIntensity;
    public Color TerrainLightColor;

    private AudioClip _ambientSound;
    private AudioSource _audioSource;
    private Vector2 _size;
    private Vector2 _position;
    private float _scale;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
    }
    void Apply()
    {
        if (_ambientSound != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.clip = _ambientSound;
            _audioSource.Play();
        }
        SetCollider();
        SetLighting();
    }
    public void SetBasicSettings(string guid, Vector2 position, Vector2 size, float scale)
    {
        chamberGuid = guid;
        _position = position;
        _size = size;
        _scale = scale;
        SetCollider();
    }

    private void SetCollider()
    {
        var minX = _position.x;
        var maxX = _position.x + _size.x;
        var biasY = 50f -_size.y;
        var minY = _position.y + biasY;
        var maxY = _position.y + _size.y + biasY;
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

    private void SetLighting()
    {
        var backgroundLightGameObject = GameObject.FindGameObjectWithTag("BackgroundLight");
        var backgroundLight = backgroundLightGameObject.GetComponent<Light2D>();
        backgroundLight.intensity = BackgroundLightIntensity;
        backgroundLight.color = BackgroundLightColor;
        var terrainLightGameObject = GameObject.FindGameObjectWithTag("TerrainLight");
        var terrainLight = backgroundLightGameObject.GetComponent<Light2D>();
        terrainLight.intensity = TerrainLightIntensity;
        terrainLight.color = TerrainLightColor;
    }
}
