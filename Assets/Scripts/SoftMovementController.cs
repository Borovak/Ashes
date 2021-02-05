using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftMovementController : MonoBehaviour
{
    public Vector2 amplitude;
    public float period = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Move(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move(bool phase)
    {
        LeanTween.moveLocal(gameObject, phase ? amplitude : -amplitude, period).setEaseInOutSine().setOnComplete(() => Move(!phase));
    }
}
