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
    }

    private struct MainMenuButtonInfo
    {
        internal string text;
        internal Action actionOnClick;
    }

    public ButtonIds id;
    public int contextIndex;

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
        };
        SetText();
    }

    void OnEnable()
    {
        MenuController.OnSelect += OnSelect;
    }

    void OnDisable()
    {
        MenuController.OnSelect -= OnSelect;
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
        _buttonBack.color = contextIndex == MenuController.index ? _colorWhenSelected : _colorWhenUnselected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuController.index = contextIndex;
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
        SceneManager.LoadScene("Game");
    }

    private void OnSelect()
    {
        if (contextIndex != MenuController.index) return;
        ExecuteAction();
    }

    private void ExecuteAction()
    {
        if (!_buttons.TryGetValue(id, out var buttonInfo)) return;
        buttonInfo.actionOnClick?.Invoke();
    }
}
