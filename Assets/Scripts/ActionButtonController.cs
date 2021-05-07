using Classes;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    public Constants.ControllerButtons button;
    public Image imageControl;
    public UIControllerButton uiControllerButton;
    public GameObject quantityGameObject;
    public TextMeshProUGUI quantityTextControl;
    public Constants.IconElementTypes iconElementType;
    public IIconElement iconElement;

    // Start is called before the first frame update
    void Start()
    {
         uiControllerButton.button = button;
        switch (iconElementType)
        {
            case Constants.IconElementTypes.Action:
                quantityGameObject.SetActive(false);
                OnActionAssignmentChanged();
                break;
            case Constants.IconElementTypes.Item:
                quantityGameObject.SetActive(true);
                OnItemAssignmentChanged();
                OnInventoryChanged();
                break; 
        }
    }

    void OnEnable()
    {
        switch (iconElementType)
        {
            case Constants.IconElementTypes.Action:
                ActionAssignmentController.AssignmentChanged += OnActionAssignmentChanged;
                OnActionAssignmentChanged();
                break;
            case Constants.IconElementTypes.Item:
                ItemAssignmentController.AssignmentChanged += OnItemAssignmentChanged;
                GameController.PlayerSpawned += RegisterPlayerInventory;
                RegisterPlayerInventory(null);
                OnItemAssignmentChanged();
                OnInventoryChanged();
                break; 
        }
    }

    void OnDisable()
    {
        switch (iconElementType)
        {
            case Constants.IconElementTypes.Action:
                ActionAssignmentController.AssignmentChanged -= OnActionAssignmentChanged;
                break;
            case Constants.IconElementTypes.Item:
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
        imageControl.sprite = iconElement?.sprite ?? ActionAssignmentController.GetArt(button, transform);
    }

    private void OnItemAssignmentChanged()
    {
        imageControl.gameObject.SetActive(imageControl.sprite != null);
        quantityGameObject.SetActive(imageControl.sprite != null);
        OnInventoryChanged();
    }

    private void OnInventoryChanged()
    {
        iconElement = ItemAssignmentController.GetItemBundle(button);
        imageControl.sprite = iconElement?.sprite ?? ItemAssignmentController.GetArt(button);
        quantityTextControl.text = iconElement?.quantity.ToString() ?? "";
    }
}
