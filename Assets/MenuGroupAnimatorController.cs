using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGroupAnimatorController : MonoBehaviour
{

    private Animator _animator;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
        MenuInputs.SelectionChangeLeft += OnSelectionChangeLeft;
        MenuInputs.SelectionChangeRight += OnSelectionChangeRight;
    }
    
    void OnDisable()
    {
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
        MenuInputs.SelectionChangeLeft -= OnSelectionChangeLeft;
        MenuInputs.SelectionChangeRight -= OnSelectionChangeRight;
    }

    private void OnSelectionChangeUp()
    {
        Move("Up");
    }
    private void OnSelectionChangeDown()
    {
        Move("Down");
    }
    private void OnSelectionChangeLeft()
    {
        Move("Left");
    }
    private void OnSelectionChangeRight()
    {
        Move("Right");
    }

    private void Move(string selectedMoveName)
    {
        var moveNames = new[] { "Up", "Down", "Left", "Right" };
        foreach (var moveName in moveNames)
        {
            if (moveName == selectedMoveName)
            {
                _animator.SetTrigger(moveName);
            }
            else
            {
                _animator.SetBool(moveName, false);
            }
        }
    }
}
