using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ChamberController : MonoBehaviour
{
    public const float unitSize = 50f;
    public Vector2 normalizedSize = new Vector2(1, 1);
    public Vector2 size => normalizedSize * unitSize;
    public int chamberId;
    public float BackgroundLightIntensity;
    public Color BackgroundLightColor;
    public float TerrainLightIntensity;
    public Color TerrainLightColor;
    public AudioClip _ambientSound;
    public Light2D backgroundLight;
    public Light2D terrainLight;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            virtualCamera.Follow = player.transform;
        }
        if (_ambientSound != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.clip = _ambientSound;
            _audioSource.Play();
        }
        SetLighting();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetLighting()
    {
        backgroundLight.intensity = BackgroundLightIntensity;
        backgroundLight.color = BackgroundLightColor;
        terrainLight.intensity = TerrainLightIntensity;
        terrainLight.color = TerrainLightColor;
    }
}
