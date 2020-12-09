using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class InteractionController : MonoBehaviour
{
    public string guid;
    public TextMeshProUGUI text;
    public abstract Constants.InteractionTypes interactionType { get; }
    public abstract string interactionText { get; }
    public abstract void Interact();
    
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
