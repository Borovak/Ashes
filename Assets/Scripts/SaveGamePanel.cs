using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGamePanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public static int currentIndex = -1;
    public int index;

    private Image _panel;
    private GameObject _newGame;
    private GameObject _zoneName;
    private GameObject _gameTime;
    private GameObject _doubleJump;

    // Start is called before the first frame update
    void Start()
    {
        _panel = GetComponent<Image>();
        _newGame = transform.Find("NewGame").gameObject;
        _zoneName = transform.Find("ZoneName").gameObject;
        _gameTime = transform.Find("GameTime").gameObject;
        _doubleJump = transform.Find("DoubleJump").gameObject;
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        var color = _panel.color;
        color.a = currentIndex == index ? 0.05f : 0.025f;
        _panel.color = color;
    }

    public void Load()
    {
        SaveSystem.index = index;
        transform.Find("Id").GetComponent<Text>().text = $"Save {index + 1}";
        var data = SaveSystem.Load(false);
        _newGame.SetActive(data == null);
        _zoneName.SetActive(data != null);
        _gameTime.SetActive(data != null);
        _doubleJump.SetActive(data != null && data.HasDoubleJump);
        for (int i = 0; i < 12; i++)
        {
            transform.Find($"Heart{i}").gameObject.SetActive(data != null && i < data.MaxHp);
        }
        if (data != null)
        {
            _zoneName.GetComponent<Text>().text = data.CampsiteLocation[0].ToString();
            var t = data.GameTime;
            var h = Convert.ToInt32(t / 3600f);
            t -= h * 3600f;
            var m = Convert.ToInt32(t / 60f);
            _gameTime.GetComponent<Text>().text = $"{h}:{m.ToString("00")}";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SaveSystem.index = index;
        SceneManager.LoadScene("Main");
    }
}
