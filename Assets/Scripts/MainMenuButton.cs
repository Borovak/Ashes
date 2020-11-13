using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public enum ButtonIds
    {
        None = -1,
        LoadCreate = 0,
        Options = 1,
        Exit = 2,
        Back = 3,
        LoadSave1 = 4,
        LoadSave2 = 5,
        LoadSave3 = 6,
        Resume = 7,
        ExitToMenu = 8,
        ExitToDesktop = 9,
        DeleteSave1 = 10,
        DeleteSave2 = 11,
        DeleteSave3 = 12,
        Inventory = 13,
        Map = 14,
    }

    private struct MainMenuButtonInfo
    {
        internal string text;
        internal Action actionOnClick;
    }

    public ButtonIds id;
    public int contextIndex;
    public int subContextIndex;

    private Color _colorWhenSelected = new Color(1f, 1f, 1f, 0.1f);
    private Color _colorWhenUnselected = new Color(1f, 1f, 1f, 0.02f);
    private Image _buttonBack;
    private Animator _animator;
    private Dictionary<ButtonIds, MainMenuButtonInfo> _buttons;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Animator>();
        _buttonBack = GetComponent<Image>();
        _buttons = new Dictionary<ButtonIds, MainMenuButtonInfo>
        {
            {ButtonIds.LoadCreate, new MainMenuButtonInfo{text = "Start", actionOnClick = () => MenuNavigation(0)}},
            {ButtonIds.Options, new MainMenuButtonInfo{text = "Options", actionOnClick = () => MenuNavigation(1)}},
            {ButtonIds.Exit, new MainMenuButtonInfo{text = "Exit", actionOnClick = Application.Quit}},
            {ButtonIds.Back, new MainMenuButtonInfo{text = "Back", actionOnClick = () => _animator.SetTrigger("Back")}},
            {ButtonIds.LoadSave1, new MainMenuButtonInfo{text = "Save 1", actionOnClick = () => LoadSaveFile(0)}},
            {ButtonIds.LoadSave2, new MainMenuButtonInfo{text = "Save 2", actionOnClick = () => LoadSaveFile(1)}},
            {ButtonIds.LoadSave3, new MainMenuButtonInfo{text = "Save 3", actionOnClick = () => LoadSaveFile(2)}},
            {ButtonIds.Resume, new MainMenuButtonInfo{text = "Resume", actionOnClick = () => _animator.SetTrigger("Back")}},
            {ButtonIds.ExitToMenu, new MainMenuButtonInfo{text = "Exit to Menu", actionOnClick = () => SceneManager.LoadScene("MainMenu")}},
            {ButtonIds.ExitToDesktop, new MainMenuButtonInfo{text = "Exit to Desktop", actionOnClick = Application.Quit}},
            {ButtonIds.DeleteSave1, new MainMenuButtonInfo{text = "X", actionOnClick = () => ConfirmDeletion(0)}},
            {ButtonIds.DeleteSave2, new MainMenuButtonInfo{text = "X", actionOnClick = () => ConfirmDeletion(1)}},
            {ButtonIds.DeleteSave3, new MainMenuButtonInfo{text = "X", actionOnClick = () => ConfirmDeletion(2)}},
            {ButtonIds.Inventory, new MainMenuButtonInfo{text = "Inventory"}},
            {ButtonIds.Map, new MainMenuButtonInfo{text = "Map"}},
        };
        SetText();
    }

    void OnEnable()
    {
        //MenuController.OnOK += OnOK;
    }

    void OnDisable()
    {
        //MenuController.OnOK -= OnOK;
    }

    private void SetText()
    {
        if (!_buttons.TryGetValue(id, out var buttonInfo)) return;
        var textComponent = GetComponentInChildren<Text>();
        textComponent.text = buttonInfo.text.ToUpper();
    }

    // Update is called once per frame
    void Update()
    {
        _buttonBack.color = contextIndex == MenuController.index && subContextIndex == MenuController.subContextIndex ? _colorWhenSelected : _colorWhenUnselected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuController.index = contextIndex;
        MenuController.subContextIndex = subContextIndex;
        _animator.SetInteger("Index", Convert.ToInt32(id));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteAction();
    }

     private void MenuNavigation(int index)
    {
        _animator.SetInteger("Index", index);
        _animator.SetTrigger("Select");
    }

    private void LoadSaveFile(int index)
    {
        SaveSystem.index = index;
        var dataPresent = GetComponent<SaveGamePanel>().dataPresent;
        if (!dataPresent)
        {
            SaveSystem.Save("", true, out _);
        }
        if (SaveSystem.Load(out var data, out var errorMessage))
        {
            SceneManager.LoadScene("LevelDesignerLoader");
        }
        else
        {
            Debug.Log(errorMessage);
        }
    }

    private void OnSelect()
    {
        if (contextIndex != MenuController.index || subContextIndex != MenuController.subContextIndex) return;
        ExecuteAction();
    }

    private void ExecuteAction()
    {
        if (!_buttons.TryGetValue(id, out var buttonInfo)) return;
        buttonInfo.actionOnClick?.Invoke();
    }

    private void ConfirmDeletion(int index)
    {
        SaveSystem.Delete(index);
    }
}
