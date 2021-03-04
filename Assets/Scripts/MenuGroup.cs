using UI;
using UnityEngine;

public class MenuGroup : MonoBehaviour
{
    public bool isNavigable;

    void OnEnable()
    {
        if (!isNavigable) return;
        MenuInputs.SelectionChangeUp += OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown += OnSelectionChangeDown;
    }

    void OnDisable()
    {
        if (!isNavigable) return;
        MenuInputs.SelectionChangeUp -= OnSelectionChangeUp;
        MenuInputs.SelectionChangeDown -= OnSelectionChangeDown;
    }

    void Start()
    {
        SelectableItem.SelectMin();
    }
    
    private void OnSelectionChangeUp()
    {
        if (!isNavigable) return;
        SelectableItem.SelectionUp();
    }

    private void OnSelectionChangeDown()
    {
        if (!isNavigable) return;
        SelectableItem.SelectionDown();
    }

}
