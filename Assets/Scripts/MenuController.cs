﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public static int index;
    public static int subContextIndex;
    public static event Action OnOK;
    public List<Vector2> choices => _choices.ToList();

    private Animator _animator;
    private MenuInputs _inputs;
    private List<Vector2> _choices;
    private int _maxIndex;
    private int _maxSubIndex;

    // Start is called before the first frame update
    void Start()
    {
        _choices = new List<Vector2>();
        _animator = GetComponent<Animator>();
        _inputs = GetComponent<MenuInputs>();
        _inputs.Start += StartPressed;
        _inputs.Select += SelectPressed;
        _inputs.OK += OKPressed;
        _inputs.Back += BackPressed;
        _inputs.SelectionChangeUp += MoveUpPressed;
        _inputs.SelectionChangeDown += MoveDownPressed;
        _inputs.SelectionChangeLeft += MoveLeftPressed;
        _inputs.SelectionChangeRight += MoveRightPressed;
    }

    void OnEnable()
    {
        index = 0;
        subContextIndex = 0;
    }

    void OnDisable()
    {
        if (_inputs == null) return;
        _inputs.OK -= OKPressed;
        _inputs.Back -= BackPressed;
        _inputs.Start -= StartPressed;
        _inputs.Select -= SelectPressed;
        _inputs.SelectionChangeUp -= MoveUpPressed;
        _inputs.SelectionChangeDown -= MoveDownPressed;
        _inputs.SelectionChangeLeft -= MoveLeftPressed;
        _inputs.SelectionChangeRight -= MoveRightPressed;
    }

    private void BackPressed()
    {
        _animator.SetTrigger("Back");
    }

    void Update()
    {
        var menuButtons = GameObject.FindGameObjectsWithTag("MenuButton");
        var tempChoices = new List<Vector2>();
        foreach (var menuButtonObject in menuButtons)
        {
            var menuButton = menuButtonObject.GetComponent<MainMenuButton>();
            tempChoices.Add(new Vector2(menuButton.contextIndex, menuButton.subContextIndex));
        }
        _choices = tempChoices;
        if (tempChoices.Count > 0)
        {
            _maxIndex = Convert.ToInt32(tempChoices.Max(x => x.x));
            var subIndexChoices = tempChoices.Where(x => x.x == index).ToList();
            _maxSubIndex = subIndexChoices.Count > 0 ? Convert.ToInt32(subIndexChoices.Max(x => x.y)) : 0;
        }
        else
        {
            _maxIndex = 0;
            _maxSubIndex = 0;
        }
    }

    private void StartPressed()
    {
        _animator.SetBool("OK", false);
        _animator.SetBool("Back", false);
        _animator.SetTrigger("Start");
    }

    private void SelectPressed()
    {
        _animator.SetTrigger("Select");
    }

    private void OKPressed()
    {
        OnOK?.Invoke();
        _animator.SetTrigger("OK");
    }

    private void MoveUpPressed()
    {
        index = index + 1 > _maxIndex ? index = 0 : index + 1;
        subContextIndex = 0;
        _animator.SetBool("OK", false);
        _animator.SetInteger("Index", index);
    }

    private void MoveDownPressed()
    {
        index = index - 1 < 0 ? index = _maxIndex : index - 1;
        subContextIndex = 0;
        _animator.SetBool("OK", false);
        _animator.SetInteger("Index", index);
    }

    private void MoveLeftPressed()
    {
        subContextIndex = subContextIndex + 1 > _maxSubIndex ? _maxSubIndex = 0 : subContextIndex + 1;
        _animator.SetBool("OK", false);
    }

    private void MoveRightPressed()
    {
        subContextIndex = subContextIndex - 1 < 0 ? _maxSubIndex : subContextIndex - 1;
        _animator.SetBool("OK", false);
    }
}
