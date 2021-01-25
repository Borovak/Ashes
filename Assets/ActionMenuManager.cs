using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenuManager : MonoBehaviour
{
    public GameObject[] sectionButtons;
    public GameObject[] sections;

    private static int _sectionIndex;
    private static int _previousSectionIndex = -1;
    private static int _sectionCount;
    private MenuGroup _menuGroup;

    // Start is called before the first frame update
    void Start()
    {
        _menuGroup = GetComponent<MenuGroup>();
        _sectionCount = sections.Length;
        MenuInputs.SectionPrevious += SectionPrevious;
        MenuInputs.SectionNext += SectionNext;
    }

    // Update is called once per frame
    void Update()
    {
        if (_sectionIndex == _previousSectionIndex) return;
        _previousSectionIndex = _sectionIndex;
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].SetActive(i == _sectionIndex);
            _menuGroup.ActiveButton = sectionButtons[_sectionIndex].GetComponent<MenuButton>();
        }
    }
    void OnEnable()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].SetActive(i == _sectionIndex);
        }
    }

    void OnDisable()
    {
        foreach (var section in sections)
        {
            section.SetActive(false);
        }
    }

    public static void ChangeSection(int index)
    {
        _sectionIndex = index;
    }

    public static void SectionNext()
    {
        _sectionIndex = _sectionIndex + 1 < _sectionCount ? _sectionIndex + 1 : 0;
    }

    public static void SectionPrevious()
    {
        _sectionIndex = _sectionIndex - 1 >= 0 ? _sectionIndex - 1 : _sectionCount - 1;
    }

}
