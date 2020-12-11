using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogController : MonoBehaviour, IPointerClickHandler
{
    public static event Action<IDialogItem> DialogUpdated;

    public IDialogItem dialogItem;
    public Image panel;
    public Image foliage;
    public Image imageNpcPortrait;
    public TextMeshProUGUI textNpcName;
    public TextMeshProUGUI textDialog;

    // Start is called before the first frame update
    void Start()
    {
        DialogUpdated += RefreshDialog;
        ClearDialog();
    }

    private void RefreshDialog(IDialogItem newDialogItem) 
    {
        dialogItem = newDialogItem;
        panel.enabled = true;
        foliage.enabled = true;
        imageNpcPortrait.enabled = true;
        imageNpcPortrait.sprite = dialogItem.npcSprite;
        textNpcName.text = dialogItem.npcName;
        textDialog.text = dialogItem.text;
    }

    private void ClearDialog()
    {
        panel.enabled = false;
        foliage.enabled = false;
        imageNpcPortrait.enabled = false;
        textNpcName.text = "";
        textDialog.text = "";
    }

    public static void UpdateDialog(IDialogItem dialogItem)
    {
        DialogUpdated?.Invoke(dialogItem);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClearDialog();
        dialogItem?.ActionOnOk?.Invoke();
    }
}
