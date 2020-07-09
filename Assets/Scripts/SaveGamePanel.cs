using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGamePanel : MonoBehaviour
{
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
    }

    public void Load()
    {
        SaveSystem.index = index;
        transform.Find("Id").GetComponent<Text>().text = $"Save {index + 1}";
        var loadMessage = SaveSystem.Load();
        var data = SaveSystem.latestSaveData;
        _newGame.SetActive(data == null);
        _zoneName.SetActive(data != null);
        _gameTime.SetActive(data != null);
        _doubleJump.SetActive(data != null && data.HasDoubleJump);
        if (data != null)
        {
            _zoneName.GetComponent<Text>().text = data.ZoneName;
            var t = data.GameTime;
            var h = Convert.ToInt32(t / 3600f);
            t -= h * 3600f;
            var m = Convert.ToInt32(t / 60f);
            _gameTime.GetComponent<Text>().text = $"{h}:{m.ToString("00")}";
        }
    }

}
