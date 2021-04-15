using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    public Constants.ControllerButtons button;
    public GameObject imageGameObject;
    public UIControllerButton uiControllerButton;

    private GameObject _artGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        uiControllerButton.button = button;
        _artGameObject = ActionAssignmentController.GetArtObject(button, transform, imageGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
