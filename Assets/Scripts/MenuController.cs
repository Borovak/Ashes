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
    private List<Vector2> _choices;
    private int _maxIndex;
    private int _maxSubIndex;

    // Start is called before the first frame update
    void Start()
    {
        _choices = new List<Vector2>();
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        MenuInputs.Start += StartPressed;
        MenuInputs.Select += SelectPressed;
        MenuInputs.OK += OKPressed;
        MenuInputs.Back += BackPressed;
        MenuInputs.SelectionChangeUp += MoveUpPressed;
        MenuInputs.SelectionChangeDown += MoveDownPressed;
        MenuInputs.SelectionChangeLeft += MoveLeftPressed;
        MenuInputs.SelectionChangeRight += MoveRightPressed;
        MenuInputs.Crafting += CraftingPressed;
        MenuInputs.Map += MapPressed;
    }

    void OnDisable()
    {
        MenuInputs.OK -= OKPressed;
        MenuInputs.Back -= BackPressed;
        MenuInputs.Start -= StartPressed;
        MenuInputs.Select -= SelectPressed;
        MenuInputs.SelectionChangeUp -= MoveUpPressed;
        MenuInputs.SelectionChangeDown -= MoveDownPressed;
        MenuInputs.SelectionChangeLeft -= MoveLeftPressed;
        MenuInputs.SelectionChangeRight -= MoveRightPressed;
        MenuInputs.Crafting -= CraftingPressed;
        MenuInputs.Map -= MapPressed;
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

    private void CraftingPressed()
    {
        ActionMenuManager.ChangeSection(0);
        SelectPressed();
    }

    private void MapPressed()
    {
        ActionMenuManager.ChangeSection(1);
        SelectPressed();
    }
}
