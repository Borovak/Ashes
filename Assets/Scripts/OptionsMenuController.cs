using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Classes;
using Static;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
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
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private List<OptionItemController> _optionItemControllers;
    private int _index;

    void Awake()
    {
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
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed += OnOkPressed;
        SelectableItem.SelectedItemChanged += OnSelectedItemChanged;
    }

    void OnDisable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed -= OnOkPressed;
        SelectableItem.SelectedItemChanged -= OnSelectedItemChanged;
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

    private void OnSelectedItemChanged(SelectableItem selectedItem)
    {
        Index = selectedItem?.index ?? -1;
    }

}
