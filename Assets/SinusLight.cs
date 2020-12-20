using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SinusLight : MonoBehaviour
{

    public AnimationCurve intensityCurve;
    public float maxIntensity;
    public float rate;

    private Light2D _light2D;
    private float _progress;
    
    // Start is called before the first frame update
    void Start()
    {
        _progress = UnityEngine.Random.Range(0f,1f);
        _light2D = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _progress += Time.deltaTime * rate;
        _light2D.intensity = intensityCurve.Evaluate(_progress % 1f) * maxIntensity;
    }
}
