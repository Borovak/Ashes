using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ChamberController : MonoBehaviour
{
    public const float unitSize = 50f;
    public Vector2 normalizedSize = new Vector2(1f, 1f);
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
        if (_ambientSound != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.clip = _ambientSound;
            _audioSource.Play();
        }
        SetCollider();
        SetLighting();
        LocationManager.currentChamberId = chamberId;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetCollider()
    {
        var collider = GetComponent<PolygonCollider2D>();
        collider.points[0] = new Vector2(0f, 0f);
        collider.points[1] = new Vector2(25f, 0f);
        collider.points[2] = new Vector2(size.x * unitSize, 0f);
        collider.points[3] = new Vector2(size.x * unitSize, size.y * unitSize);
        collider.points[4] = new Vector2(0f, size.y * unitSize);
    }

    private void SetLighting()
    {
        backgroundLight.intensity = BackgroundLightIntensity;
        backgroundLight.color = BackgroundLightColor;
        terrainLight.intensity = TerrainLightIntensity;
        terrainLight.color = TerrainLightColor;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(size.x, size.y, 0f) / 2f, size);
    }
}
