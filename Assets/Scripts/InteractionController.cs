using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using Player;
using TMPro;
using UnityEngine;

public abstract class InteractionController : MonoBehaviour
{
    public string guid
    {
        get
        {
            if (!string.IsNullOrEmpty(forcedGuid)) return forcedGuid;
            if (string.IsNullOrEmpty(_guid)){
                _guid = System.Guid.NewGuid().ToString();
            }
            return _guid;
        }
        set => _guid = value;
    }
    
    public TextMeshProUGUI text;
    public abstract Constants.InteractionTypes interactionType { get; }
    public abstract string interactionText { get; }
    public abstract void Interact();
    public string forcedGuid;

    private string _guid;

    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInteractionController.Instance == null && text != null) return;
        text.text = PlayerInteractionController.Instance.interactionController == this ? interactionText : "";
    }
}
