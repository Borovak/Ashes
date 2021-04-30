using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Static;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour
{
    [Serializable]
    public enum CanvasModes
    {
        Game,
        Shop,
        RadialSelection,
        Cutscene,
        SystemMenuRoot,
        SystemMenuOptions,
        ActionMenuCrafting,
        ActionMenuMap,
        ActionMenuInventory,
        Dialog
    }

    public static CanvasModes CanvasMode
    {
        get => _canvasMode;
        set => ChangeCanvasMode(value);
    }
    
    public static event Action OnOk;
    public List<Vector2> Choices => _choices.ToList();
    public GameObject gameUI;
    public GameObject shopPanelPrefab;
    public GameObject dialogPanelPrefab;
    public GameObject cutscenePanelPrefab;

    private static MenuController _instance;
    private static CanvasModes _canvasMode;
    private static Animator _animator;
    private List<Vector2> _choices;
    private int _maxIndex;
    private int _maxSubIndex;

    // Start is called before the first frame update
    void Start()
    {
        _choices = new List<Vector2>();
        _animator = GetComponent<GameController>().canvas.GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetInteger("ActionMenuIndex", ActionMenuManager.sectionIndex);
    }

    void OnEnable()
    {
        _instance = this;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed += OkPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed += BackPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.Start].Pressed += StartPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.Select].Pressed += SelectPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.LT].StateChanged += OnButtonLTStateChanged;
        MenuInputs.SelectionChangeUp += MoveUpPressed;
        MenuInputs.SelectionChangeDown += MoveDownPressed;
        MenuInputs.SelectionChangeLeft += MoveLeftPressed;
        MenuInputs.SelectionChangeRight += MoveRightPressed;
        MenuInputs.Inventory += InventoryPressed;
        MenuInputs.Crafting += CraftingPressed;
        MenuInputs.Map += MapPressed;
    }

    void OnDisable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed -= OkPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.B].Pressed -= BackPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.Start].Pressed -= StartPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.Select].Pressed -= SelectPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.LT].StateChanged -= OnButtonLTStateChanged;
        MenuInputs.SelectionChangeUp -= MoveUpPressed;
        MenuInputs.SelectionChangeDown -= MoveDownPressed;
        MenuInputs.SelectionChangeLeft -= MoveLeftPressed;
        MenuInputs.SelectionChangeRight -= MoveRightPressed;
        MenuInputs.Inventory -= InventoryPressed;
        MenuInputs.Crafting -= CraftingPressed;
        MenuInputs.Map -= MapPressed;
    }

    private void OnButtonLTStateChanged(bool state)
    {
        _animator.SetBool("RadialSelection", state);
    }

    private void BackPressed()
    {
        _animator.SetTrigger("Back");
    }

    private void StartPressed()
    {
        _animator.SetBool("OK", false);
        _animator.SetBool("Back", false);
        _animator.SetBool("Select", false);
        _animator.SetBool("Start", true);
    }

    private void SelectPressed()
    {
        _animator.SetBool("OK", false);
        _animator.SetBool("Back", false);
        _animator.SetBool("Start", false);
        _animator.SetBool("Select", true);
    }

    private void OkPressed()
    {
        OnOk?.Invoke();
    }

    private void MoveUpPressed()
    {
        _animator.SetBool("OK", false);
    }

    private void MoveDownPressed()
    {
        _animator.SetBool("OK", false);
    }

    private void MoveLeftPressed()
    {
        _animator.SetBool("OK", false);
    }

    private void MoveRightPressed()
    {
        _animator.SetBool("OK", false);
    }

    private void InventoryPressed()
    {
        _animator.SetTrigger("Inventory");
        ActionMenuManager.ChangeSection(0);
        //SelectPressed();
    }

    private void CraftingPressed()
    {
        _animator.SetTrigger("Crafting");
        ActionMenuManager.ChangeSection(1);
        //SelectPressed();
    }

    private void MapPressed()
    {
        _animator.SetTrigger("Map");
        ActionMenuManager.ChangeSection(2);
        //SelectPressed();
    }

    private static void ChangeCanvasMode(CanvasModes canvasMode)
    {
        var previousCanvasMode = _canvasMode;
        _canvasMode = canvasMode;
        //Changing what is enabled
        _instance.gameUI.SetActive(canvasMode == CanvasModes.Game);
        //Setting specific parameters on exit state
        switch (previousCanvasMode)
        {
            case CanvasModes.Shop:
                GlobalShopManager.currentShopId = -1;
                break;
        }
    }

    public static void OpenDialog()
    {
        _animator.SetTrigger("Dialog");
    }
    
    public static void OpenShop(int shopId)
    {
        GlobalShopManager.currentShopId = shopId;
        _animator.SetTrigger("Shop");
    }

    public static void ReturnToGame()
    {
        _animator.SetTrigger("Back");
    }

    public static void OpenCutscene(CutsceneController.Cutscenes cutscene, Action actionOnEnd)
    {
        CutsceneController.Init(cutscene, actionOnEnd);
        FadeInOutController.FadeOut(() => _animator.SetTrigger("Cutscene"));
    }
}
