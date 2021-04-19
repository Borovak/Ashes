using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Player;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    private enum Types
    {
        None,
        Action,
        Item
    }
    
    public Constants.ControllerButtons button;
    public GameObject imageGameObject;
    public UIControllerButton uiControllerButton;
    public GameObject quantityGameObject;
    public TextMeshProUGUI quantityTextControl;

    private Types _type;


    private void Awake()
    {
        if (new[] {Constants.ControllerButtons.X, Constants.ControllerButtons.B, Constants.ControllerButtons.LB, Constants.ControllerButtons.RB}.Contains(button))
        {
            _type = Types.Action;
        } 
        else if (new[] {Constants.ControllerButtons.DUp, Constants.ControllerButtons.DDown, Constants.ControllerButtons.DLeft, Constants.ControllerButtons.DRight}.Contains(button))
        {
            _type = Types.Item;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiControllerButton.button = button;
        switch (_type)
        {
            case Types.Action:
                quantityGameObject.SetActive(false);
                OnActionAssignmentChanged();
                break;
            case Types.Item:
                quantityGameObject.SetActive(true);
                OnItemAssignmentChanged();
                OnInventoryChanged();
                break; 
        }
    }

    void OnEnable()
    {
        switch (_type)
        {
            case Types.Action:
                ActionAssignmentController.AssignmentChanged += OnActionAssignmentChanged;
                break;
            case Types.Item:
                ItemAssignmentController.AssignmentChanged += OnItemAssignmentChanged;
                GameController.PlayerSpawned += RegisterPlayerInventory;
                RegisterPlayerInventory(null);
                break; 
        }
    }

    void OnDisable()
    {
        switch (_type)
        {
            case Types.Action:
                ActionAssignmentController.AssignmentChanged -= OnActionAssignmentChanged;
                break;
            case Types.Item:
                ItemAssignmentController.AssignmentChanged -= OnItemAssignmentChanged;
                GameController.PlayerSpawned -= RegisterPlayerInventory;
                if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
                inventory.InventoryChanged -= OnInventoryChanged;
                break; 
        }
    }

    private void RegisterPlayerInventory(GameObject playerGameObject)
    {
        if (!GlobalInventoryManager.TryGetInventory(-1, out var inventory)) return;
        inventory.InventoryChanged += OnInventoryChanged;
        OnInventoryChanged();
    }
    
    private void OnActionAssignmentChanged()
    {
        ActionAssignmentController.GetArtObject(button, transform, imageGameObject);
    }

    private void OnItemAssignmentChanged()
    {
        var image = imageGameObject.GetComponent<Image>();
        image.sprite = ItemAssignmentController.GetArt(button);
        imageGameObject.SetActive(image.sprite != null);
        quantityGameObject.SetActive(image.sprite != null);
    }

    private void OnInventoryChanged()
    {
        quantityTextControl.text = ItemAssignmentController.GetQuantity(button).ToString();
    }
}
