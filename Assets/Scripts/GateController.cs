using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public static Dictionary<string, bool> gates;
    public string id;
    public float timeToOpen;
    public float positionOpened;
    public float positionClosed;
    public bool state => gates.TryGetValue(id, out var value) ? value : false;
    public bool previousState;
    private float desiredPosition => state ? positionOpened : positionClosed;

    void Awake()
    {
        if (gates == null){
            gates = new Dictionary<string, bool>();
        }
    }

    void Start(){
        
        if (!gates.ContainsKey(id))
        {
            gates.Add(id, false);
        }
    }

    void OnEnable()
    {
        if (state != previousState)
        {
            var y = desiredPosition;
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            previousState = state;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(desiredPosition - transform.localPosition.y) > 0.1f)
        {
            var totalDistance = state ? positionOpened - positionClosed : positionClosed - positionOpened;
            var distancePerSecond = totalDistance / Mathf.Max(0.01f, timeToOpen);
            var maxMove = distancePerSecond * Time.deltaTime;
            transform.Translate(Vector3.up * (state ? Mathf.Min(desiredPosition - transform.localPosition.y, maxMove) : Mathf.Max(desiredPosition - transform.localPosition.y, maxMove)));
        }
        previousState = state;
    }
}
