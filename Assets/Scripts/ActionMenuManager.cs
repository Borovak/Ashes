using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;

public class ActionMenuManager : MonoBehaviour
{
    public GameObject[] sectionButtons;

    public static int sectionIndex;
    private static int _previousSectionIndex = -1;
    private static int _sectionCount;
    private MenuGroup _menuGroup;

    // Start is called before the first frame update
    void Start()
    {
        _menuGroup = GetComponent<MenuGroup>();
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
        for (int i = 0; i < sectionButtons.Length; i++)
        {            
            _menuGroup.ActiveButton = sectionButtons[sectionIndex].GetComponent<MenuButton>();
        }
    }

    public static void ChangeSection(int index)
    {
        sectionIndex = index;
    }

    public static void SectionNext()
    {
        sectionIndex = sectionIndex + 1 < _sectionCount ? sectionIndex + 1 : 0;
    }

    public static void SectionPrevious()
    {
        sectionIndex = sectionIndex - 1 >= 0 ? sectionIndex - 1 : _sectionCount - 1;
    }

}
