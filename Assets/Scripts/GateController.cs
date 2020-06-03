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
    public bool state => gates[id];

    void Awake()
    {
        if (gates == null)
        {
            gates = new Dictionary<string, bool>();
        }
        if (!gates.ContainsKey(id))
        {
            gates.Add(id, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var desiredPosition = state ? positionOpened : positionClosed;
        if (Mathf.Abs(desiredPosition - transform.localPosition.y) > 0.1f)
        {
            var totalDistance = state ? positionOpened - positionClosed : positionClosed - positionOpened;
            var distancePerSecond = totalDistance / Mathf.Max(0.01f, timeToOpen);
            var maxMove = distancePerSecond * Time.deltaTime;
            transform.Translate(Vector3.up * (state ? Mathf.Min(desiredPosition - transform.localPosition.y, maxMove) : Mathf.Max(desiredPosition - transform.localPosition.y, maxMove)));
        }
    }
}
