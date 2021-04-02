using System.Collections;
using System.Collections.Generic;
using Classes;
using Static;
using UnityEngine;

public class ActionMenuManager : MonoBehaviour
{
    public GameObject[] sectionButtons;

    public static int sectionIndex;
    private static int _sectionCount;
    private int _previousSectionIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        _sectionCount = sectionButtons.Length;
    }

    void OnEnable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.LB].Pressed += SectionPrevious;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.RB].Pressed += SectionNext;
    }

    void OnDisable()
    {
        ControllerInputs.controllerButtons[Constants.ControllerButtons.LB].Pressed -= SectionPrevious;
        ControllerInputs.controllerButtons[Constants.ControllerButtons.RB].Pressed -= SectionNext;
    }

    // Update is called once per frame
    void Update()
    {
        if (sectionIndex == _previousSectionIndex) return;
        _previousSectionIndex = sectionIndex;
    }

    public static void ChangeSection(int index)
    {
        sectionIndex = index;
    }

    public void SectionNext()
    {
        sectionIndex = sectionIndex + 1 < _sectionCount ? sectionIndex + 1 : 0;
        GetComponent<MenuFunctions>().SetCanvasTrigger("SectionNext");
    }

    public void SectionPrevious()
    {
        sectionIndex = sectionIndex - 1 >= 0 ? sectionIndex - 1 : _sectionCount - 1;
        GetComponent<MenuFunctions>().SetCanvasTrigger("SectionPrevious");
    }

}
