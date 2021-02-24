using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Interfaces;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogController : MonoBehaviour, IPointerClickHandler
{
    public static event Action<IDialogItem> DialogUpdated;
    public static event Action<Action> ActionToBeExecuted;
    public static event Action<int> SelectedDialogChanged;
    public static bool isVisible;
    public static bool inDialog;

    public IDialogItem dialogItem;
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
        Index = 0;
        foreach (var p in dialogChoicePanels)
        {
            p.SelectionChanged += UpdateIndex;
            p.DialogClicked += ExecuteDialog;
        }
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed += OnButtonPressed;
    }
    
    void OnDisable()
    {
        foreach (var p in dialogChoicePanels)
        {
            p.SelectionChanged -= UpdateIndex;
            p.DialogClicked -= ExecuteDialog;
        }
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.A].Pressed -= OnButtonPressed;
    }
    
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

    private void UpdateIndex(int i)
    {
        Index = i;
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

    private void OnButtonPressed()
    {
        _choices[Index].Value.Invoke();
    }

    private void ExecuteDialog(int index)
    {
        _choices[index].Value.Invoke();
    }

    private void SetFollowUp()
    {
        if (dialogItem is IDialogItemChoices dialogItemChoices)
        {
            _choices = dialogItemChoices.followUp.ToList();
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
        inDialog = true;
    }

    private void ClearDialog()
    {
        panel.enabled = false;
        foliage.enabled = false;
        imageNpcPortrait.enabled = false;
        textNpcName.text = "";
        textDialog.text = "";
        isVisible = false;
        inDialog = false;
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

    private void OnSelectionChangeDown()
    {
        Index = Index >= _choices.Count - 1 ? 0 : Index + 1;
    }

    private void OnSelectionChangeUp()
    {
        Index = Index == 0 ? _choices.Count - 1 : Index - 1;
    }
}
