using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static event Action<DialogItem> DialogUpdated;

    public DialogItem dialogItem;
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

    private void RefreshDialog(DialogItem dialogItem) 
    {
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

    public static void UpdateDialog(DialogItem dialogItem)
    {
        DialogUpdated?.Invoke(dialogItem);
    }
}
