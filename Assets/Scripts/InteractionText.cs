using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionText : MonoBehaviour
{

    private Action<Vector3> _setPosition;
    private UnityEngine.UI.Text _textComponent;

    // Start is called before the first frame update
    void Start()
    {
        _setPosition = GetComponent<PlaceUIElementAtWorldPosition>().MoveToClickPoint;
        _textComponent = GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _textComponent.text = PlayerInteractionController.Instance.interactionPossible ? PlayerInteractionController.Instance.interactionText : "";
        if (PlayerInteractionController.Instance.interactionPossible){
            _setPosition(PlayerInteractionController.Instance.interactionPosition);
        }
    }
}
