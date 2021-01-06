using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFoliageRotator : MonoBehaviour
{
    public float range = 2f;
    public float time = 2f;

    private float _min;
    private float _max;

    // Start is called before the first frame update
    void Start()
    {
        _min = transform.eulerAngles.z - range;
        _max = transform.eulerAngles.z + range;
        var ratio = UnityEngine.Random.Range(0f, 1f);
        var startPoint = ratio * range + _min;
        LeanTween.rotateZ(gameObject, _max, (1f - ratio) * time).setEaseOutSine().setOnComplete(() => NextRotation(_min));
    }

    private void NextRotation(float target)
    {
        LeanTween.rotateZ(gameObject, target, time).setEaseInOutSine().setOnComplete(() => NextRotation(target == _min ? _max : _min));
    }
}
