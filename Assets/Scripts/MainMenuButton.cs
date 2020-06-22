using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    public static int currentIndex;
    public int index;
    public Color colorWhenSelected;
    public GameObject SaveGamePanel;

    private List<Image> _images;

    // Start is called before the first frame update
    void Start()
    {
        _images = new List<Image>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i);
            if (c.name != "SelectionDot") continue;
            var image = c.GetComponent<Image>();
            _images.Add(image);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var image in _images)
        {
            var color = image.color;
            color = colorWhenSelected;
            color.a = index == currentIndex ? colorWhenSelected.a : 0f;
            image.color = color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (index)
        {
            case 0:
                SaveGamePanel.SetActive(true);
                transform.parent.gameObject.SetActive(false);
                break;
            case 1:
                break;
            case 2:
                Application.Quit();
                break;
        }
    }
}
