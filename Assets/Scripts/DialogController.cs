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
    public static event Action<Action> ActionToBeExecuted;
    public static bool isVisible;

    public IDialogItem dialogItem;
    public Image panel;
    public Image foliage;
    public Image imageNpcPortrait;
    public TextMeshProUGUI textNpcName;
    public TextMeshProUGUI textDialog;
    public DialogChoicePanel[] dialogChoicePanels;

    private Vector3 _initialPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        DialogUpdated += RefreshDialog;
        ActionToBeExecuted += OnActionToBeExecuted;
        ClearDialog();
    }

    void Update()
    {
        if (dialogItem != null && GlobalFunctions.TryGetPlayer(out var player) ? Vector3.Distance(player.transform.position, _initialPlayerPosition) > 2f : false)
        {
            ClearDialog();
        }
    }

    private void RefreshDialog(IDialogItem newDialogItem)
    {
        _initialPlayerPosition = GlobalFunctions.TryGetPlayer(out var player) ? player.transform.position : Vector3.zero;
        dialogItem = newDialogItem;
        panel.enabled = true;
        foliage.enabled = true;
        imageNpcPortrait.enabled = true;
        imageNpcPortrait.sprite = dialogItem.npcSprite;
        textNpcName.text = dialogItem.npcName;
        textDialog.text = dialogItem.text;
        isVisible = true;
        SetFollowUp();
    }

    private void SetFollowUp()
    {
        var dialogItemChoices = dialogItem as IDialogItemChoices;
        if (dialogItemChoices != null)
        {
            var choices = dialogItemChoices.followUp.ToList();
            for (int i = 0; i < dialogChoicePanels.Length; i++)
            {
                var isActive = i < choices.Count;
                dialogChoicePanels[i].gameObject.SetActive(isActive);
                if (isActive)
                {
                    dialogChoicePanels[i].UpdateContent(choices[i].Key, choices[i].Value);
                }
            }
        }
        else
        {
            DesactivateChoices();
        }
    }

    private void ClearDialog()
    {
        panel.enabled = false;
        foliage.enabled = false;
        imageNpcPortrait.enabled = false;
        textNpcName.text = "";
        textDialog.text = "";
        isVisible = false;
        DesactivateChoices();
    }

    private void DesactivateChoices()
    {
        foreach (var choice in dialogChoicePanels)
        {
            choice.gameObject.SetActive(false);
        }
    }

    public static void UpdateDialog(IDialogItem dialogItem)
    {
        DialogUpdated?.Invoke(dialogItem);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var dialogItemSimple = dialogItem as IDialogItemSimple;
        if (dialogItemSimple == null) return;
        ClearDialog();
        dialogItemSimple.ActionOnOk?.Invoke();
    }

    public static void DoAction(Action action)
    {
        ActionToBeExecuted?.Invoke(action);
    }

    public void OnActionToBeExecuted(Action action)
    {
        ClearDialog();
        action.Invoke();
    }
}
