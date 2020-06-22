using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{

    public string id;
    public float timeToMove = 1f;
    public Vector3 positionPressed => positionUnpressed + (Vector3.down * 0.2f);
    public Vector3 positionUnpressed;
    public bool state => GateController.gates.TryGetValue(id, out var value) ? value : false;
    public Vector3 desiredPosition => state ? positionPressed : positionUnpressed;
    public bool? _previousState;

    // Start is called before the first frame update
    void Start()
    {
        positionUnpressed = transform.localPosition;
        if (GateController.gates == null)
        {
            GateController.gates = new Dictionary<string, bool>();
        }
        if (!GateController.gates.ContainsKey(id))
        {
            GateController.gates.Add(id, false);
        }
        if (state){
            transform.localPosition = positionPressed;
            _previousState = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_previousState.HasValue && _previousState == state) return;
        _previousState = state;
        StartCoroutine(MoveToPosition(transform, desiredPosition, timeToMove));
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.localPosition;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.localPosition = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var triggerCollider = GetComponent<BoxCollider2D>();
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
