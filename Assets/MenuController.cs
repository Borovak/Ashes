using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public static int index;
    public static event Action OnSelect;
    public int maxIndex;

    private Animator _animator;
    private MenuInputs _inputs;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _inputs = GetComponent<MenuInputs>();
        _inputs.Back += () => _animator.SetTrigger("Back");
        _inputs.Start += StartPressed;
        _inputs.Select += SelectPressed;
        _inputs.SelectionChangePositive += MoveUpPressed;
        _inputs.SelectionChangeNegative += MoveDownPressed;
    }

    void OnEnable()
    {
        index = 0;
    }

    private void StartPressed()
    {
        _animator.SetBool("Select", false);
        _animator.SetBool("Back", false);
        _animator.SetTrigger("Start");
    }

    private void SelectPressed()
    {
        OnSelect?.Invoke();
        _animator.SetTrigger("Select");
    }

    private void MoveUpPressed()
    {
        index = index + 1 > maxIndex ? index = 0 : index + 1;
        _animator.SetBool("Select", false);
        _animator.SetInteger("Index", index);
    }

    private void MoveDownPressed()
    {
        index = index - 1 < 0 ? index = maxIndex : index - 1;
        _animator.SetBool("Select", false);
        _animator.SetInteger("Index", index);
    }
}
