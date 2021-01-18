using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogChoicePanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public static event Action<DialogChoicePanel> SelectedDialogChoiceChanged;
    public Color color;
    public TextMeshProUGUI textControl;

    private Action _onClick;
    private Image _image;
    private readonly Color _colorTransparent = new Color(0f, 0f, 0f, 0f);


    public void UpdateContent(string text, Action onClick)
    {
        textControl.text = text;
        _onClick = onClick;
        OnSelectedDialogChoiceChanged(null);
    }


    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        OnSelectedDialogChoiceChanged(null);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable()
    {
        SelectedDialogChoiceChanged += OnSelectedDialogChoiceChanged;
    }

    void OnDisable()
    {
        SelectedDialogChoiceChanged -= OnSelectedDialogChoiceChanged;
    }

    private void OnSelectedDialogChoiceChanged(DialogChoicePanel selectedPanel)
    {
        var backColor = selectedPanel == this ? color : _colorTransparent;
        var foreColor = selectedPanel == this ? Color.black : color;
        _image = GetComponent<Image>();
        _image.color = backColor;
        textControl.color = foreColor;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SelectedDialogChoiceChanged?.Invoke(this);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SelectedDialogChoiceChanged?.Invoke(this);
    }
}
