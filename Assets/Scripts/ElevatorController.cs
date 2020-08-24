using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public enum States {
        ToA,
        ToB,
        StandbyToA, 
        StandbyToB
    }
    public Vector3 positionA;
    public Vector3 positionB;
    public float speed = 1f;
    public float standbyTimeAtDestination = 0f;
    public States state;

    private float _standbyTime;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state){
            case States.ToA:
                MoveTo(positionA, States.StandbyToB);
                break;
            case States.ToB:
                MoveTo(positionB, States.StandbyToA);
                break;
            case States.StandbyToA:
                Wait(States.ToA);
                break;
            case States.StandbyToB:
                Wait(States.ToB);
                break;
        }
        
    }

    private void MoveTo(Vector3 destination, States followUpState){
        _rb.MovePosition(Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime));
        if (Vector3.Distance(transform.position, destination) < 0.001f){
            state = followUpState;
            _standbyTime = standbyTimeAtDestination;
        }
    }

    private void Wait(States followUpState){
        _standbyTime -= Time.deltaTime;
        if (_standbyTime <= 0)
        {            
            state = followUpState;
        }
    }
}
