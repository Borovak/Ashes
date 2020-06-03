using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{

    public string id;
    public float timeToPress;
    public float positionPressed;
    public float positionUnpressed;
    public bool state => GateController.gates[id];

    // Start is called before the first frame update
    void Start()
    {
        if (GateController.gates == null)
        {
            GateController.gates = new Dictionary<string, bool>();
        }
        if (!GateController.gates.ContainsKey(id)){
            GateController.gates.Add(id, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var desiredPosition = state ? positionPressed : positionUnpressed;
        if (Mathf.Abs(desiredPosition - transform.localPosition.y) > 0.001f)
        {
            var totalDistance = state ? positionPressed - positionUnpressed : positionUnpressed - positionPressed;
            var distancePerSecond = totalDistance / Mathf.Max(0.0001f, timeToPress);
            var maxMove = distancePerSecond * Time.deltaTime;
            transform.Translate(Vector3.up * (state ? Mathf.Min(desiredPosition - transform.localPosition.y, maxMove) : Mathf.Max(desiredPosition - transform.localPosition.y, maxMove)));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var triggerCollider = GetComponents<BoxCollider2D>().FirstOrDefault(x => x.isTrigger);
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
        SetState(true);
    }

    public void SetState(bool newState)
    {
        GateController.gates[id] = newState;
    }
}
