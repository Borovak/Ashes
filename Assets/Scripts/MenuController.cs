using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : MonoBehaviour
{
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

    private void StartPressed()
    {
        _animator.SetBool("OK", false);
        _animator.SetBool("Back", false);
        _animator.SetBool("Select", false);
        _animator.SetBool("Start", true);
    }

    private void SelectPressed()
    {
        _animator.SetBool("OK", false);
        _animator.SetBool("Back", false);
        _animator.SetBool("Start", false);
        _animator.SetBool("Select", true);
    }

    private void OKPressed()
    {
        OnOK?.Invoke();
        _animator.SetTrigger("OK");
    }

    private void MoveUpPressed()
    {
        _animator.SetBool("OK", false);
    }

    private void MoveDownPressed()
    {
        _animator.SetBool("OK", false);
    }

    private void MoveLeftPressed()
    {
        _animator.SetBool("OK", false);
    }

    private void MoveRightPressed()
    {
        _animator.SetBool("OK", false);
    }
}
