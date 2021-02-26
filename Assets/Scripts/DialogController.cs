using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Dialog;
using Interfaces;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static event Action<IDialogItem> DialogUpdated;
    public static event Action<Action> ActionToBeExecuted;
    public static event Action<int> SelectedDialogChanged;
    public static IDialogItem DialogItem;
    
    public Image panel;
    public Image foliage;
    public Image imageNpcPortrait;
    public TextMeshProUGUI textNpcName;
    public TextMeshProUGUI textDialog;
    public DialogChoicePanel[] dialogChoicePanels;

    public int Index
    {
        get => _index;
        set
        {
            _index = value;
            SelectedDialogChanged?.Invoke(Index);
        }
    }

    private Vector3 _initialPlayerPosition;
    private int _index;
    private List<KeyValuePair<string, Action>> _choices;

    void OnEnable()
    {
        _choices = new List<KeyValuePair<string, Action>>();
        foreach (var p in dialogChoicePanels)
        {
            p.SelectionChanged += UpdateIndex;
            p.DialogClicked += ExecuteDialog;
        }
        DialogUpdated += RefreshDialog;
        ActionToBeExecuted += OnActionToBeExecuted;
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed += OnButtonPressed;
        UpdateDialog(DialogItem);
    }
    
    void OnDisable()
    {
        foreach (var p in dialogChoicePanels)
        {
            p.SelectionChanged -= UpdateIndex;
            p.DialogClicked -= ExecuteDialog;
        }
        DialogUpdated -= RefreshDialog;
        ActionToBeExecuted -= OnActionToBeExecuted;
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed -= OnButtonPressed;
    }

    void Update()
    {
        if (DialogItem != null && GlobalFunctions.TryGetPlayer(out var player) && Vector3.Distance(player.transform.position, _initialPlayerPosition) > 2f)
        {
            MenuController.ReturnToGame();
        }
    }

    private void UpdateIndex(int i)
    {
        Index = i;
    }

    private void RefreshDialog(IDialogItem newDialogItem)
    {
        Index = 0;
        _initialPlayerPosition = GlobalFunctions.TryGetPlayer(out var player) ? player.transform.position : Vector3.zero;
        DialogItem = newDialogItem;
        panel.enabled = true;
        foliage.enabled = true;
        imageNpcPortrait.enabled = true;
        imageNpcPortrait.sprite = DialogItem.NpcSprite;
        textNpcName.text = DialogItem.NpcName;
        textDialog.text = DialogItem.Text;
        SetFollowUp();
    }

    private void OnButtonPressed()
    {
        if (Index < 0 || Index >= _choices.Count) return;
        _choices[Index].Value.Invoke();
    }

    private void ExecuteDialog(int index)
    {
        _choices[index].Value.Invoke();
    }

    private void SetFollowUp()
    {
        if (DialogItem is IDialogItemChoices dialogItemChoices)
        {
            _choices = dialogItemChoices.FollowUp.ToList();
            for (int i = 0; i < dialogChoicePanels.Length; i++)
            {
                var isActive = i < _choices.Count;
                dialogChoicePanels[i].gameObject.SetActive(isActive);
                if (isActive)
                {
                    dialogChoicePanels[i].UpdateContent(_choices[i].Key, i);
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
        if (dialogItem == null)
        {
            MenuController.ReturnToGame();
        }
        DialogUpdated?.Invoke(dialogItem);
    }

    public static void DoAction(Action action)
    {
        ActionToBeExecuted?.Invoke(action);
    }

    private void OnActionToBeExecuted(Action action)
    {
        ClearDialog();
        action.Invoke();
    }

    private void OnSelectionChangeDown()
    {
        if (Index + 1 < _choices.Count)
        {
            Index++;
        }
    }

    private void OnSelectionChangeUp()
    {
        if (Index - 1 >= 0)
        {
            Index--;
        }
    }
}
