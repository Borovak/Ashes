using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLayoutImpact : MonoBehaviour
{
    public bool alsoOnWater;
    public GameObject splashPrefab;
    public GameObject waterSplashPrefab;
    public AudioClip[] contactSounds;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var hit = Physics2D.OverlapCircle(transform.position, transform.localScale.x / 2f, LayerManagement.Layout.value + (alsoOnWater ? LayerManagement.Water.value : 0));
        if (hit != null)
        {
            if (splashPrefab != null)
            {
                var splashPrefabUsed = hit.IsTouchingLayers(LayerManagement.Water.value) && waterSplashPrefab != null ? waterSplashPrefab : splashPrefab;
                var splash = GameObject.Instantiate<GameObject>(splashPrefabUsed, transform.position, Quaternion.identity);
                GameObject.Destroy(splash, 2f);
            }
            AudioFunctions.PlayRandomSound(contactSounds, transform.position);
            GameObject.Destroy(gameObject);
        }
    }
}
