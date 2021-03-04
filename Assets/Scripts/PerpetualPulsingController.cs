using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualPulsingController : MonoBehaviour
{
    public Vector3 scaleA;
    public Vector3 scaleB;
    public float period = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.Framework.LeanTween.scale(gameObject, scaleA, 0f).setOnComplete(Pulse);
    }

    private void Pulse()
    {
        LeanTween.Framework.LeanTween.scale(gameObject, scaleB, period)
            .setEaseInSine()
            .setOnComplete(
            () => LeanTween.Framework.LeanTween.scale(gameObject, scaleA, period)
                .setEaseInSine()
                .setOnComplete(Pulse));
    }
}
