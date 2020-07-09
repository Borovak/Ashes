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
        _inputs.Select += Select;
        _inputs.SelectionChangePositive += MoveUp;
        _inputs.SelectionChangeNegative += MoveDown;
    }

    private void Select()
    {
        OnSelect?.Invoke();
        _animator.SetTrigger("Select");
    }

    private void MoveUp()
    {
        index = index + 1 > maxIndex ? index = 0 : index + 1;
        _animator.SetBool("Select", false);
        _animator.SetInteger("Index", index);
    }

    private void MoveDown()
    {
        index = index - 1 < 0 ? index = maxIndex : index - 1;
        _animator.SetBool("Select", false);
        _animator.SetInteger("Index", index);
    }
}
