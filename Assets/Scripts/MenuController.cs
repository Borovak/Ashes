using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Serializable]
    public enum CanvasModes
    {
        Game,
        Shop,
        Cutscene,
        SystemMenu,
        SystemMenuOptions,
        ActionMenuCrafting,
        ActionMenuMap,
        ActionMenuInventory,
    }

    public static event Action OnOK;
    public List<Vector2> choices => _choices.ToList();
    public GameObject GameUI;
    public GameObject ShopUI;
    public GameObject CutsceneUI;
    public GameObject ActionMenu;
    public GameObject ActionMenuInventory;
    public GameObject ActionMenuCrafting;
    public GameObject ActionMenuMap;
    public GameObject SystemMenu;
    public GameObject SystemMenuRoot;
    public GameObject SystemMenuOptions;

    private static MenuController _instance;
    private static CanvasModes _previousCanvasMode;
    private Animator _animator;
    private List<Vector2> _choices;
    private int _maxIndex;
    private int _maxSubIndex;

    // Start is called before the first frame update
    void Start()
    {
        _choices = new List<Vector2>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetInteger("ActionMenuIndex", ActionMenuManager.sectionIndex);
    }

    void OnEnable()
    {
        _instance = this;
        MenuInputs.Start += StartPressed;
        MenuInputs.Select += SelectPressed;
        MenuInputs.Ok += OKPressed;
        MenuInputs.Back += BackPressed;
        MenuInputs.SelectionChangeUp += MoveUpPressed;
        MenuInputs.SelectionChangeDown += MoveDownPressed;
        MenuInputs.SelectionChangeLeft += MoveLeftPressed;
        MenuInputs.SelectionChangeRight += MoveRightPressed;
        MenuInputs.Inventory += InventoryPressed;
        MenuInputs.Crafting += CraftingPressed;
        MenuInputs.Map += MapPressed;
        UIShopController.OpenShopRequired += OnOpenShopRequired;
        UIShopController.CloseShopRequired += OnCloseShopRequired;
    }

    void OnDisable()
    {
        MenuInputs.Ok -= OKPressed;
        MenuInputs.Back -= BackPressed;
        MenuInputs.Start -= StartPressed;
        MenuInputs.Select -= SelectPressed;
        MenuInputs.SelectionChangeUp -= MoveUpPressed;
        MenuInputs.SelectionChangeDown -= MoveDownPressed;
        MenuInputs.SelectionChangeLeft -= MoveLeftPressed;
        MenuInputs.SelectionChangeRight -= MoveRightPressed;
        MenuInputs.Inventory -= InventoryPressed;
        MenuInputs.Crafting -= CraftingPressed;
        MenuInputs.Map -= MapPressed;
        UIShopController.OpenShopRequired -= OnOpenShopRequired;
        UIShopController.CloseShopRequired -= OnCloseShopRequired;
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

    private void OKPressed()
    {
        OnOK?.Invoke();
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

    public static void ChangeCanvasMode(CanvasModes canvasMode)
    {
        if (_previousCanvasMode == CanvasModes.Shop)
        {
            GlobalShopManager.currentShopId = -1;
        }
        _previousCanvasMode = canvasMode;
        var menuObjects = new List<GameObject> { _instance.ActionMenu, _instance.ActionMenuInventory, _instance.ActionMenuCrafting, _instance.ActionMenuMap, _instance.SystemMenu, _instance.SystemMenuRoot, _instance.SystemMenuOptions, _instance.GameUI, _instance.ShopUI, _instance.CutsceneUI };
        var menuObjectsForEveryCanvasMode = new Dictionary<CanvasModes, List<GameObject>>{
            {CanvasModes.Game, new List<GameObject> {_instance.GameUI}},
            {CanvasModes.Shop, new List<GameObject> {_instance.ShopUI}},            
            {CanvasModes.Cutscene, new List<GameObject> {_instance.CutsceneUI}},
            {CanvasModes.SystemMenu, new List<GameObject> {_instance.SystemMenu, _instance.SystemMenuRoot}},
            {CanvasModes.SystemMenuOptions, new List<GameObject> {_instance.SystemMenu, _instance.SystemMenuOptions}},
            {CanvasModes.ActionMenuInventory, new List<GameObject> {_instance.ActionMenu, _instance.ActionMenuInventory}},
            {CanvasModes.ActionMenuCrafting, new List<GameObject> {_instance.ActionMenu, _instance.ActionMenuCrafting}},
            {CanvasModes.ActionMenuMap, new List<GameObject> {_instance.ActionMenu, _instance.ActionMenuMap}},
        };
        if (!menuObjectsForEveryCanvasMode.TryGetValue(canvasMode, out var menuObjectsForCurrentCanvasMode))
        {
            menuObjectsForCurrentCanvasMode = new List<GameObject>();
        }
        foreach (var menuObject in menuObjects)
        {
            menuObject.SetActive(menuObjectsForCurrentCanvasMode.Contains(menuObject));
        }

    }

    private void OnOpenShopRequired()
    {
        _animator.SetTrigger("Shop");
    }

    private void OnCloseShopRequired()
    {
        _animator.SetTrigger("Back");
    }
}
