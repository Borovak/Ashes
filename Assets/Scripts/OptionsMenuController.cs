using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Classes;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{

    public static event Action<int> SelectedIndexChanged;
    public GameObject OptionItemPrefab;
    public GameObject TogglePrefab;
    public GameObject SliderPrefab;
    public GameObject SectionNamePrefab;
    public GameObject BackButton;
    public float initialY = -500f;
    public float interval = 100f;

    public int Index
    {
        get => _index;
        set 
        {
            _index = value;
            SelectedIndexChanged?.Invoke(_index);
            EventSystem.current.SetSelectedGameObject(null);
            _menuGroup.HoveredButton = _index == _optionItemControllers.Count ? BackButton.GetComponent<MenuButton>() : null;
            // if (_index < _optionItemControllers.Count)
            // {
            //     var optionItemController = _optionItemControllers[_index];
            //     if (optionItemController.gameOption.type == typeof(float))
            //     {
            //         var parentObject = optionItemController.gameObject;
            //         var childObject = parentObject.GetComponentInChildren<Slider>().gameObject;
            //         Debug.Log($"{_index}: {parentObject}, {childObject}");
            //     }
            // }
        }
    }

    private List<OptionItemController> _optionItemControllers;
    private MenuGroup _menuGroup;
    public int _index;

    void Awake()
    {
        _menuGroup = GetComponent<MenuGroup>();
        _optionItemControllers = new List<OptionItemController>();
    }
    
    void OnEnable()
    {
        _optionItemControllers.Clear();
        GlobalFunctions.DeleteChildrenInTransform(transform, BackButton);
        if (!GameOptionsManager.TryGetAllOptions(out var gameOptions))
        {
            gameObject.SetActive(false);
            return;
        }
        var y = initialY;
        var i = 0;
        var previousSection = "";
        foreach (var gameOption in gameOptions)
        {
            if (gameOption.section != previousSection)
            {
                var sectionObject = Instantiate(SectionNamePrefab, transform);
                var sectionObjectRectTransform = sectionObject.GetComponent<RectTransform>();
                var sectionObjectPosition = sectionObjectRectTransform.anchoredPosition;
                sectionObjectPosition.y = y;
                sectionObjectRectTransform.anchoredPosition = sectionObjectPosition;
                var sectionObjectText = sectionObject.GetComponent<TextMeshProUGUI>();
                sectionObjectText.text = gameOption.section;
                previousSection = gameOption.section;
                y += interval;
            }
            var optionItem = Instantiate(OptionItemPrefab, transform);
            optionItem.name = gameOption.name;
            var optionItemRectTransform = optionItem.GetComponent<RectTransform>();
            var position = optionItemRectTransform.anchoredPosition;
            position.y = y;
            optionItemRectTransform.anchoredPosition = position;
            AddSubControl(gameOption.type, optionItem);
            var optionItemController = optionItem.GetComponent<OptionItemController>();
            optionItemController.GameOption = gameOption;
            _optionItemControllers.Add(optionItemController);
            y += interval;
            i++;
        }
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DUp].Pressed += OnDUpPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DDown].Pressed += OnDDownPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed += OnOkPressed;
        Index = 0;
    }

    void OnDisable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DUp].Pressed -= OnDUpPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.DDown].Pressed -= OnDDownPressed;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed -= OnOkPressed;
    }

    private void OnDUpPressed()
    {
        if (Index == 0)
        {
            Index = _optionItemControllers.Count;
        }
        else
        {
            Index -= 1;
        }
    }

    private void OnDDownPressed()
    {
        if (Index == _optionItemControllers.Count)
        {
            Index = 0;
        }
        else
        {
            Index += 1;
        }
    }

    private void OnOkPressed()
    {
        if (Index == _optionItemControllers.Count) return;
        var optionItemController = _optionItemControllers[Index];
        if (optionItemController.GameOption.type == typeof(bool))
        {
            optionItemController.GameOption.value = optionItemController.GameOption.value == true.ToString() ? false.ToString() : true.ToString();
        }
    }

    private void AddSubControl(Type type, GameObject optionItem)
    {
        var prefabs = new Dictionary<Type, GameObject>
        {
            {typeof(bool), TogglePrefab},
            {typeof(float), SliderPrefab},
        };
        if (!prefabs.TryGetValue(type, out var prefab)) return;
        Instantiate(prefab, optionItem.transform);
    }

}
