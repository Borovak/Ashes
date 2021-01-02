using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuController : MonoBehaviour
{

    public GameObject OptionItemPrefab;
    public GameObject TogglePrefab;
    public GameObject SliderPrefab;
    public GameObject BackButton;
    public float initialY = -500f;
    public float interval = 100f;

    void OnEnable()
    {
        GlobalFunctions.DeleteChildrenInTransform(transform, BackButton);
        if (!GameOptionsManager.TryGetAllOptions(out var gameOptions))
        {
            gameObject.SetActive(false);
            return;
        }
        var y = initialY;
        foreach (var gameOption in gameOptions)
        {
            var optionItem = GameObject.Instantiate(OptionItemPrefab, transform);
            var optionItemRectTransform = optionItem.GetComponent<RectTransform>();
            var position = optionItemRectTransform.anchoredPosition;
            position.y = y;
            AddSubControl(gameOption.type, optionItem);
            optionItemRectTransform.anchoredPosition = position;
            var optionItemController = optionItem.GetComponent<OptionItemController>();
            optionItemController.gameOption = gameOption;
            y += interval;
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
        GameObject.Instantiate(prefab, optionItem.transform);
    }

}
