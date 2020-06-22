using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationStartPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var animator = GetComponent<Animator>();
        animator.Play(0,-1, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
