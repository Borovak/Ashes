using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogChoicePanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public event Action<int> SelectionChanged;
    public event Action<int> DialogClicked;
    public TextMeshProUGUI textControl;

    private Image _image;
    private int _index;
    private readonly Color _colorSelected = new Color(1f, 1f, 0f, 1f);
    private readonly Color _colorTransparent = new Color(0f, 0f, 0f, 0f);

    public void UpdateContent(string text, int index)
    {
        textControl.text = text;
        _index = index;
        UpdateControls(index == 0);
    }

    private void OnEnable()
    {
        DialogController.SelectedDialogChanged += OnSelectedDialogChanged;
    }

    private void OnDisable()
    {
        DialogController.SelectedDialogChanged -= OnSelectedDialogChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    private void OnSelectedDialogChanged(int selectedIndex)
    {
        UpdateControls(_index == selectedIndex);
    }

    private void UpdateControls(bool isSelected)
    {
        var backColor = isSelected ? _colorSelected : _colorTransparent;
        var foreColor = isSelected ? Color.black : _colorSelected;
        _image = GetComponent<Image>();
        _image.color = backColor;
        textControl.color = foreColor;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SelectionChanged?.Invoke(_index);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        DialogClicked?.Invoke(_index);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SelectionChanged?.Invoke(_index);
    }
}
